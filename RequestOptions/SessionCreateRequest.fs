namespace OpenTokFs.RequestOptions

/// <summary>
/// An object that provides parameters for creating an OpenTok session.
/// </summary>
type SessionCreateRequest() =
    member val ArchiveAlways = false with get, set
    member val IpAddressLocationHint: string = null with get, set
    member val P2PEnabled: bool = false with get, set