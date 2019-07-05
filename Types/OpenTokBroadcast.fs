namespace ISchemm.OpenTokFs.Types

open System.Collections.Generic

type OpenTokRtmpStream = {
    serverUrl: string
    streamName: string
    status: string
}

type OpenTokBroadcastStreams = {
    hls: string
    rtmp: Dictionary<string, OpenTokRtmpStream>
}

type OpenTokBroadcast = {
    id: string
    sessionId: string
    projectId: int
    createdAt: int64
    updatedAt: int64
    resolution: string
    broadcastUrls: OpenTokBroadcastStreams
    status: string
}