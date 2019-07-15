namespace OpenTokFs

open System
open System.Net
open System.Security.Cryptography

// http://www.fssnip.net/7RK/title/Creating-and-validating-JWTs-in-just-35-lines-of-F-code
module internal JsonWebToken =
    open System.Text
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

    let CreateToken(credentials: IOpenTokCredentials) =
        let payload = {
            iss = credentials.ApiKey.ToString()
            ist = "project"
            iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            exp = DateTimeOffset.UtcNow.AddMinutes(3.0).ToUnixTimeSeconds()
            jti = Guid.NewGuid().ToString()
        }
        let encodedSecret = credentials.ApiSecret |> System.Text.Encoding.UTF8.GetBytes
        let testAuth = JsonWebToken.newJwtAuthority (fun key -> new HMACSHA256(key) :> HMAC) encodedSecret
        let header = 
            """{
                "alg": "HS256",
                "typ": "JWT"
            }"""
        testAuth.IssueToken header (Newtonsoft.Json.JsonConvert.SerializeObject payload)

    let BuildRequest (credentials: IOpenTokCredentials) (path: string) (query: seq<string>) =
        let req =
            String.concat "&" query
            |> sprintf "https://api.opentok.com/v2/project/%d/%s?%s" credentials.ApiKey path
            |> WebRequest.CreateHttp
        req.Headers.Add("X-OPENTOK-AUTH", CreateToken credentials)
        req.Headers.Add("Accept", "application/json")
        req