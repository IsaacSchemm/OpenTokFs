namespace OpenTokFs.RequestDomain

open System.Net

[<RequireQualifiedAccess>]
type ArchiveMode = Always | Manual

[<RequireQualifiedAccess>]
type P2PPreference = Enabled | Disabled

type SessionCreationParameters = {
    archiveMode: ArchiveMode
    location: IPAddress option
    p2p_preference: P2PPreference
} with
    static member Default = {
        archiveMode = ArchiveMode.Manual
        location = None
        p2p_preference = P2PPreference.Disabled
    }
    member this.QueryString = String.concat "&" [
        match this.archiveMode with
        | ArchiveMode.Always -> "archiveMode=always"
        | ArchiveMode.Manual -> "archiveMode=manual"
        match this.location with
        | Some ip -> string ip
        | None -> ()
        match this.p2p_preference with
        | P2PPreference.Enabled -> "p2p.preference=enabled"
        | P2PPreference.Disabled -> "p2p.preference=disabled"
    ]