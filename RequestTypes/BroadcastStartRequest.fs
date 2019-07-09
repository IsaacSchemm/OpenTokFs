namespace OpenTokFs.RequestTypes

open System

/// <summary>
/// An object that provides parameters for starting an OpenTok broadcast, using reasonable defaults.
/// Note that neither HLS nor RTMP is enabled by default; you will have to turn one or both of them on.
/// </summary>
type BroadcastStartRequest(sessionId: string) =
    member __.SessionId = sessionId
    member val Layout = VideoLayout.BestFit with get, set
    member val Duration = TimeSpan.FromHours 2.0 with get, set
    member val Hls = false with get, set
    member val Rtmp = Seq.empty<RtmpDestination> with get, set
    member val Resolution = "640x480" with get, set