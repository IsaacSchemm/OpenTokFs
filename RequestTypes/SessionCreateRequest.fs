namespace OpenTokFs.RequestTypes

/// <summary>
/// Any object that provides parameters for creating an OpenTok session.
/// </summary>
type ISessionCreateRequest =
    abstract member ArchiveAlways: bool
    abstract member IpAddressLocationHint: string
    abstract member BypassMediaRouter: bool

/// <summary>
/// An object that provides parameters for creating an OpenTok session.
/// </summary>
type SessionCreateRequest() =
    member val ArchiveAlways = false with get, set
    member val IpAddressLocationHint: string = null with get, set
    member val BypassMediaRouter = false with get, set
    interface ISessionCreateRequest with
        member this.ArchiveAlways = this.ArchiveAlways
        member this.IpAddressLocationHint = this.IpAddressLocationHint
        member this.BypassMediaRouter = this.BypassMediaRouter