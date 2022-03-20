namespace OpenTokFs.RequestTypes

open System.Collections.Generic
open Newtonsoft.Json

module OpenTokDialRequest =
    [<AllowNullLiteral>]
    type Credentials () =
        member val username = "" with get, set
        member val password = "" with get, set

    type SipInfo() =
        member val uri = "" with get, set

        [<JsonProperty(NullValueHandling = NullValueHandling.Ignore)>]
        member val from = null with get, set

        member val headers = Dictionary<string, string> () with get, set

        [<JsonProperty(NullValueHandling = NullValueHandling.Ignore)>]
        member val auth: Credentials = null with get, set

        member val secure = false with get, set

type OpenTokDialRequest() =
    member val sessionId = "" with get, set
    member val token = "" with get, set
    member val sip = OpenTokDialRequest.SipInfo () with get, set