namespace OpenTokFs

open System
open System.Net
open System.Security.Cryptography
open System.IO
open System.Text
open Newtonsoft.Json
open OpenTokFs.Credentials
open System.Collections.Generic

// http://www.fssnip.net/7RK/title/Creating-and-validating-JWTs-in-just-35-lines-of-F-code
module internal JsonWebToken =
    open System.Text.RegularExpressions
    let replace (oldVal: string) (newVal: string) = fun (s: string) -> s.Replace(oldVal, newVal)
    let minify = 
        let regex = Regex("(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", RegexOptions.Compiled|||RegexOptions.CultureInvariant)
        fun s ->
            regex.Replace(s, "$1")
    let base64UrlEncode bytes =
        Convert.ToBase64String(bytes) |> replace "+" "-" |> replace "/" "_" |> replace "=" ""
    type IJwtAuthority =
        inherit IDisposable
        abstract member IssueToken: header:string -> payload:string -> string
        abstract member VerifyToken: string -> bool
    let newJwtAuthority (initAlg: byte array -> HMAC) key =
        let alg = initAlg(key)
        let encode = minify >> Encoding.UTF8.GetBytes >> base64UrlEncode
        let issue header payload =
            let parts = [header; payload] |> List.map encode |> String.concat "."
            let signature = parts |> Encoding.UTF8.GetBytes |> alg.ComputeHash |> base64UrlEncode
            [parts; signature] |> String.concat "."
        let verify (token: string) =
            let secondDot = token.LastIndexOf(".")
            let parts = token.Substring(0, secondDot)
            let signature = token.Substring(secondDot + 1)
            (parts |> Encoding.UTF8.GetBytes |> alg.ComputeHash |> base64UrlEncode) = signature

        {
            new IJwtAuthority with
                member __.IssueToken header payload = issue header payload
                member __.VerifyToken token = verify token
                member __.Dispose() = alg.Dispose()
        }

/// <summary>
/// Internal functions for authenticating with OpenTok.
/// </summary>
module OpenTokAuthentication =
    type TokenBody = {
        iss: string
        ist: string
        iat: int64
        exp: int64
        jti: string
    }

    [<RequireQualifiedAccess>]
    type JwtCredentialSource = Account of IAccountCredentials | Project of IProjectCredentials

    let CreateToken (credentials: JwtCredentialSource) =
        let (key, secret, ist) =
            match credentials with
            | JwtCredentialSource.Account a -> (a.ApiKey, a.ApiSecret, "account")
            | JwtCredentialSource.Project p -> (p.ApiKey, p.ApiSecret, "project")
        let payload = {
            iss = key.ToString()
            ist = ist
            iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            exp = DateTimeOffset.UtcNow.AddMinutes(3.0).ToUnixTimeSeconds()
            jti = Guid.NewGuid().ToString()
        }
        let encodedSecret = Encoding.UTF8.GetBytes secret
        let testAuth = JsonWebToken.newJwtAuthority (fun key -> new HMACSHA256(key) :> HMAC) encodedSecret
        let header = 
            """{
                "alg": "HS256",
                "typ": "JWT"
            }"""
        testAuth.IssueToken header (Newtonsoft.Json.JsonConvert.SerializeObject payload)

    let CreateProjectToken credentials = JwtCredentialSource.Project credentials |> CreateToken
    let CreateAccountToken credentials = JwtCredentialSource.Account credentials |> CreateToken

    let BuildProjectLevelRequest (credentials: IProjectCredentials) (path: string) (query: seq<string * string>) =
        let req =
            query
            |> Seq.map (fun (k, v) -> sprintf "%s=%s" (Uri.EscapeDataString k) (Uri.EscapeDataString v))
            |> String.concat "&"
            |> sprintf "https://api.opentok.com/v2/project/%d/%s?%s" credentials.ApiKey path
            |> WebRequest.CreateHttp
        req.Headers.Add("X-OPENTOK-AUTH", CreateProjectToken credentials)
        req.Accept <- "application/json"
        req

    let BuildAccountLevelRequest (credentials: IAccountCredentials) (url: string) =
        let req = WebRequest.CreateHttp url
        req.Headers.Add("X-OPENTOK-AUTH", CreateAccountToken credentials)
        req

    let SerializeObject = JsonConvert.SerializeObject
    let DeserializeObject<'a> = JsonConvert.DeserializeObject<'a>

    let AsyncWriteJson<'a> (req: WebRequest) (map: IReadOnlyDictionary<string, obj>) = async {
        req.ContentType <- "application/json"
        use! rs = req.GetRequestStreamAsync() |> Async.AwaitTask
        use sw = new StreamWriter(rs)
        do! map |> SerializeObject |> sw.WriteLineAsync |> Async.AwaitTask
    }

    let AsyncReadJson<'a> (req: WebRequest) = async {
        use! resp = req.AsyncGetResponse()
        use s = resp.GetResponseStream()
        use sr = new StreamReader(s)
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask
        return DeserializeObject<'a> json
    }