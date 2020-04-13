namespace OpenTokFs.RequestOptions

open System

/// <summary>
/// An object that provides parameters for creating an OpenTok session.
/// </summary>
type SessionCreateRequest() =
    member val ArchiveAlways = false with get, set
    member val IpAddressLocationHint: string = null with get, set
    member val P2PEnabled: bool = false with get, set

    member internal session.ToQueryString() =
        seq {
            yield sprintf "archiveMode=%s" (if session.ArchiveAlways then "always" else "manual")
            yield sprintf "p2p.preference=%s" (if session.P2PEnabled then "enabled" else "disabled")
            if not (String.IsNullOrEmpty session.IpAddressLocationHint) then
                yield sprintf "location=%s" (Uri.EscapeDataString session.IpAddressLocationHint)
        }
        |> String.concat "&"