namespace OpenTokFs.ResponseTypes

open System

type OpenTokRtmpStream = {
    id: string // or null
    serverUrl: string
    streamName: string
    status: string
} with
    override this.ToString () = String.concat " " [
        $"{this.serverUrl}/{this.streamName}"
        if not (isNull this.id) then $"({this.id})"
        $"({this.status})"
    ]

type OpenTokBroadcastStreams = {
    hls: string // or null
    rtmp: OpenTokRtmpStream list // or null
}

type OpenTokBroadcast = {
    id: string
    sessionId: string
    projectId: int
    createdAt: int64
    broadcastUrls: OpenTokBroadcastStreams
    updatedAt: int64
    status: string
    resolution: string
} with
    member this.GetCreationTime () = DateTimeOffset.FromUnixTimeMilliseconds this.createdAt
    member this.GetUpdatedTime () = DateTimeOffset.FromUnixTimeMilliseconds this.updatedAt
    override this.ToString() = $"{this.id} ({this.status})"