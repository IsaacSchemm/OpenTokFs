namespace ISchemm.OpenTokFs.Types

open Newtonsoft.Json

type OpenTokBroadcastRequestRtmpStream = {
    serverUrl: string
    streamName: string

    [<JsonProperty(NullValueHandling=NullValueHandling.Ignore)>]
    id: string
}

type OpenTokBroadcastRequestStreams = {
    [<JsonProperty(NullValueHandling=NullValueHandling.Ignore)>]
    hls: obj

    rtmp: seq<OpenTokBroadcastRequestRtmpStream>
}

type OpenTokBroadcastRequestLayout = {
    ``type``: string
    stylesheet: string
}

type OpenTokBroadcastRequest = {
    sessionId: string
    layout: OpenTokBroadcastRequestLayout
    maxDuration: int
    outputs: OpenTokBroadcastRequestStreams
    resolution: string
}