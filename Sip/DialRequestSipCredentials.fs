namespace OpenTokFs.Sip

open Newtonsoft.Json

/// The username and password to be used in the the SIP INVITE​ request for HTTP digest authentication, if it is required by your SIP platform.
[<AllowNullLiteral>]
type DialRequestSipCredentials() =
    [<JsonProperty("username")>]
    member val Username = "" with get, set
    [<JsonProperty("password")>]
    member val Password = "" with get, set