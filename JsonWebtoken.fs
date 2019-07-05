namespace ISchemm.OpenTokFs

// Sebastian Fialka
// http://www.fssnip.net/7RK
module JsonWebToken =
    open System
    open System.Text
    open System.Text.RegularExpressions
    open System.Security.Cryptography
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