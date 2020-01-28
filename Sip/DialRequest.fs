namespace OpenTokFs.Sip

open Newtonsoft.Json

/// A request to start a SIP call.
type DialRequest() =
    /// The OpenTok session ID for the SIP call to join.
    [<JsonProperty("sessionId")>]
    member val SessionId = "" with get, set
    /// The OpenTok token to use for the participant who is being called.
    [<JsonProperty("token")>]
    member val Token = "" with get, set
    /// Information about the SIP destination.
    [<JsonProperty("sip")>]
    member val Sip = new DialRequestSip() with get, set