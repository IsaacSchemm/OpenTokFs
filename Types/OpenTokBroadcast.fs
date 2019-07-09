namespace OpenTokFs.Types

open System

/// <summary>
/// An RTMP destination for an OpenTok broadcast.
/// </summary>
type OpenTokRtmpStream = {
    id: string
    serverUrl: string
    streamName: string
    status: string
}

/// <summary>
/// Streaming endpoints for an OpenTok broadcast.
/// </summary>
type OpenTokBroadcastStreams = {
    hls: string
    rtmp: OpenTokRtmpStream[]
}

/// <summary>
/// A running OpenTok broadcast.
/// </summary>
type OpenTokBroadcast = {
    id: string
    sessionId: string
    projectId: int
    createdAt: int64
    broadcastUrls: OpenTokBroadcastStreams
    updatedAt: int64
    status: string
    maxDuration: int
    resolution: string
} with
   member this.GetCreationTime() = DateTimeOffset.FromUnixTimeMilliseconds this.createdAt
   member this.GetUpdatedTime() = DateTimeOffset.FromUnixTimeMilliseconds this.updatedAt
   member this.GetMaxDuration() = this.maxDuration |> float |> TimeSpan.FromSeconds