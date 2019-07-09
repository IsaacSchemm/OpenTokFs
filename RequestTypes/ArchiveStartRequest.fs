namespace OpenTokFs.RequestTypes

/// <summary>
/// An object that provides parameters for starting an OpenTok archive, using reasonable defaults.
/// </summary>
type ArchiveStartRequest(sessionId: string) =
    member __.SessionId = sessionId
    member val Layout = VideoLayout.BestFit with get, set
    member val HasAudio = true with get, set
    member val HasVideo = true with get, set
    member val Name = null with get, set
    member val OutputMode = "composed" with get, set
    member val Resolution = "640x480" with get, set