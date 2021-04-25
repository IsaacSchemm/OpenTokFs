namespace OpenTokFs.RequestDomain

type ArchiveMode = AutomaticallyArchive | ManuallyArchive

type SessionLocation = FromIPAddress of string | FirstClientConnectionLocation

type MediaMode = RoutedSession | RelayedSession

type NewSession = {
    archiveMode: ArchiveMode
    location: SessionLocation
    p2p_preference: MediaMode
} with
    static member Default = {
        archiveMode = ManuallyArchive
        location = FirstClientConnectionLocation
        p2p_preference = RoutedSession
    }
    member this.QueryString = String.concat "&" [
        match this.archiveMode with
        | AutomaticallyArchive -> "archiveMode=always"
        | ManuallyArchive -> "archiveMode=manual"
        match this.location with
        | FromIPAddress ip -> string ip
        | FirstClientConnectionLocation -> ()
        match this.p2p_preference with
        | RelayedSession -> "p2p.preference=enabled"
        | RoutedSession -> "p2p.preference=disabled"
    ]