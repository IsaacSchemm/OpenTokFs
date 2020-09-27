namespace OpenTokFs

open System
open System.Security.Cryptography
open System.Text
open OpenTokFs.Credentials

type OpenTokSessionTokenRole =
| Subscriber = 1
| Publisher = 2
| Moderator = 3

type OpenTokSessionTokenParameters(sessionId: string) =
    member __.SessionId = sessionId
    member val Role = OpenTokSessionTokenRole.Publisher with get, set
    member val ExpireTime = Nullable<DateTimeOffset>() with get, set
    member val Data: string = null with get, set
    member val InitialLayoutClassList = Seq.empty<string> with get, set

module OpenTokSessionTokens =
    let private R = new Random()

    let private EncodeHMAC (input: string) (key: string) =
        use hmac = new HMACSHA1(Encoding.UTF8.GetBytes key)
        let hashedValue = input |> Encoding.UTF8.GetBytes |> hmac.ComputeHash

        // convert hash to hex string, all lowercase
        hashedValue |> Seq.map (sprintf "%02x") |> String.concat ""

    let private BuildDataString (parameters: OpenTokSessionTokenParameters) (createTime: DateTimeOffset) (nonce: int) =
        seq {
            yield sprintf "session_id=%s" (Uri.EscapeDataString parameters.SessionId)
            yield sprintf "create_time=%d" (createTime.ToUnixTimeSeconds())
            yield sprintf "nonce=%d" nonce

            let role =
                match parameters.Role with
                | OpenTokSessionTokenRole.Subscriber -> "SUBSCRIBER"
                | OpenTokSessionTokenRole.Publisher -> "PUBLISHER"
                | OpenTokSessionTokenRole.Moderator -> "MODERATOR"
                | _ -> failwith "Invalid OpenTok role specified"
            yield sprintf "role=%s" role

            if not (Seq.isEmpty parameters.InitialLayoutClassList) then
                yield
                    parameters.InitialLayoutClassList
                    |> String.concat " "
                    |> Uri.EscapeDataString
                    |> sprintf "initial_layout_class_list=%s"

            match Option.ofNullable parameters.ExpireTime with
            | None -> ()
            | Some expireTime ->
                if expireTime < createTime then
                    failwith "Expiration time for token cannot be before creation time"
                if expireTime > createTime.AddDays 30.0 then
                    failwith "Expiration time cannot be more than 30 days after creation time"
                yield sprintf "expire_time=%d" (expireTime.ToUnixTimeSeconds())

            if not (String.IsNullOrEmpty parameters.Data) then
                if parameters.Data.Length > 1000 then
                    failwith "Connection data for token, if specified, cannot be more than 1000 characters"
                yield sprintf "connection_data=%s" (Uri.EscapeDataString parameters.Data)
        }
        |> String.concat "&"

    let BuildTokenString (credentials: IProjectCredentials) (dataString: string) =
        let signature = EncodeHMAC dataString credentials.ApiSecret

        sprintf "partner_id=%d&sig=%s:%s" credentials.ApiKey signature dataString
        |> Encoding.UTF8.GetBytes
        |> Convert.ToBase64String
        |> sprintf "T1==%s"

    let GenerateToken (credentials: IProjectCredentials) (parameters: OpenTokSessionTokenParameters) =
        let nonce = R.Next(0, 999999)
        let dataString = BuildDataString parameters DateTimeOffset.UtcNow nonce
        BuildTokenString credentials dataString