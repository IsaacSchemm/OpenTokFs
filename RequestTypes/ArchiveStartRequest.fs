namespace OpenTokFs.RequestTypes

/// <summary>
/// An object that provides parameters for starting an OpenTok archive, using reasonable defaults.
/// </summary>
type ArchiveStartRequest(sessionId: string) =
    member __.SessionId = sessionId
    member val Layout: VideoLayout = VideoLayout.BestFit with get, set
    member val HasAudio: bool = true with get, set
    member val HasVideo: bool  = true with get, set
    member val Name: string = null with get, set
    member val OutputMode: string = "composed" with get, set
    member val Resolution: string = "640x480" with get, set