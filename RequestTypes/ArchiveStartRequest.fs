namespace OpenTokFs.RequestTypes

/// <summary>
/// Any object that provides parameters for starting an OpenTok archive.
/// </summary>
type IArchiveStartRequest =
    abstract member SessionId: string
    abstract member HasAudio: bool
    abstract member HasVideo: bool
    abstract member Layout: VideoLayout
    abstract member Name: string
    abstract member OutputMode: string
    abstract member Resolution: string

/// <summary>
/// An object that provides parameters for starting an OpenTok archive, using reasonable defaults.
/// </summary>
type ArchiveStartRequest(sessionId: string) =
    member val Layout = VideoLayout.BestFit with get, set
    member val HasAudio = true with get, set
    member val HasVideo = true with get, set
    member val Name = null with get, set
    member val OutputMode = "composed" with get, set
    member val Resolution = "640x480" with get, set
    interface IArchiveStartRequest with
        member __.SessionId = sessionId
        member this.HasAudio = this.HasAudio
        member this.HasVideo = this.HasVideo
        member this.Layout = this.Layout
        member this.Name = this.Name
        member this.OutputMode = this.OutputMode
        member this.Resolution = this.Resolution