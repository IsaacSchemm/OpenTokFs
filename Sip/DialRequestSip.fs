namespace OpenTokFs.Sip

open Newtonsoft.Json
open System.Collections.Generic

/// Information about a SIP destination used when starting a call.
type DialRequestSip() =
    /// The SIP URI to to be used as destination of the SIP call initiated from OpenTok to your SIP platform.
    [<JsonProperty("uri")>]
    member val Uri = "" with get, set

    /// The number or string that will be sent to the final SIP number as the caller. It must be a string in the form of from@example.com, where from can be a string or a number.
    [<JsonProperty("from", NullValueHandling = NullValueHandling.Ignore)>]
    member val From: string = null with get, set

    /// Custom headers to be added to the SIP INVITE request, if any. Each of the custom headers must start with the ​"X-"​ prefix.
    [<JsonProperty("headers")>]
    member val Headers = new Dictionary<string, string>() with get, set

    /// This object contains the username and password to be used in the the SIP INVITE​ request for HTTP digest authentication, if it is required by your SIP platform.
    [<JsonProperty("auth", NullValueHandling = NullValueHandling.Ignore)>]
    member val Auth: DialRequestSipCredentials = null with get, set

    /// A Boolean flag that indicates whether the media must be transmitted encrypted or not.
    [<JsonProperty("secure", NullValueHandling = NullValueHandling.Ignore)>]
    member val Secure = false with get, set