namespace ISchemm.OpenTokFs.Types

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
    broadcastUrls: OpenTokBroadcastStreams
    updatedAt: int64
    status: string
    maxDuration: int
    resolution: string
}
