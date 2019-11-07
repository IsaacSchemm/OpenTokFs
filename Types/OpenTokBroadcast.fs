namespace OpenTokFs.Types

open System

/// <summary>
/// An RTMP destination for an OpenTok broadcast.
/// </summary>
[<AllowNullLiteral>]
type OpenTokRtmpStream() =
    member val Id: string = "" with get, set
    member val ServerUrl: string = "" with get, set
    member val StreamName: string = "" with get, set
    member val Status: string = "" with get, set

/// <summary>
/// Streaming endpoints for an OpenTok broadcast.
/// </summary>
[<AllowNullLiteral>]
type OpenTokBroadcastStreams() =
    member val Hls: string = "" with get, set
    member val Rtmp: OpenTokRtmpStream[] = Array.empty with get, set

/// <summary>
/// A running OpenTok broadcast.
/// </summary>
[<AllowNullLiteral>]
type OpenTokBroadcast() =
    member val Id: string = "" with get, set
    member val SessionId: string = "" with get, set
    member val ProjectId: int = 0 with get, set
    member val CreatedAt: int64 = 0L with get, set
    member val BroadcastUrls: OpenTokBroadcastStreams = new OpenTokBroadcastStreams() with get, set
    member val UpdatedAt: int64 = 0L with get, set
    member val Status: string = "" with get, set
    member val MaxDuration: int = 0 with get, set
    member val Resolution: string = "" with get, set

    member this.GetCreationTime() = DateTimeOffset.FromUnixTimeMilliseconds this.CreatedAt
    member this.GetUpdatedTime() = DateTimeOffset.FromUnixTimeMilliseconds this.UpdatedAt
    member this.GetMaxDuration() = this.MaxDuration |> float |> TimeSpan.FromSeconds