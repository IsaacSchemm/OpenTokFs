namespace ISchemm.OpenTokFs.Types

open System.Collections.Generic

type OpenTokRtmpStream = {
    id: string
    serverUrl: string
    streamName: string
    status: string
}

type OpenTokBroadcastStreams = {
    hls: string
    rtmp: seq<OpenTokRtmpStream>
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
