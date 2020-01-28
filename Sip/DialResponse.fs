namespace OpenTokFs.Sip

open System
open Newtonsoft.Json

/// The result of a SIP dial operation.
type DialResponse() =
    [<JsonProperty("id")>]
    member val Id = Guid.Empty with get, set
    [<JsonProperty("connectionId")>]
    member val ConnectionId = Guid.Empty with get, set
    [<JsonProperty("streamId")>]
    member val StreamId = Guid.Empty with get, set