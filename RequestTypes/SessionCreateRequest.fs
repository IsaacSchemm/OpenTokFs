namespace OpenTokFs.RequestTypes

/// <summary>
/// An object that provides parameters for creating an OpenTok session.
/// </summary>
type SessionCreateRequest() =
    member val ArchiveAlways = false with get, set
    member val IpAddressLocationHint: string = null with get, set
    member val BypassMediaRouter = false with get, set