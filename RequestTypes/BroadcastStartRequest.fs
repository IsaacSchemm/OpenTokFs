namespace OpenTokFs.RequestTypes

open System

/// <summary>
/// An object that provides parameters for starting an OpenTok broadcast, using reasonable defaults.
/// Note that neither HLS nor RTMP is enabled by default; you will have to turn one or both of them on.
/// </summary>
type BroadcastStartRequest(sessionId: string) =
    member __.SessionId = sessionId
    member val Layout: VideoLayout = VideoLayout.BestFit with get, set
    member val Duration: TimeSpan = TimeSpan.FromHours 2.0 with get, set
    member val Hls: bool = false with get, set
    member val Rtmp: RtmpDestination seq = Seq.empty with get, set
    member val Resolution: string = "640x480" with get, set