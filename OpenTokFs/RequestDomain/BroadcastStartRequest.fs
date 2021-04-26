namespace OpenTokFs.RequestDomain

open System

type DestinationId = CustomDestinationId of string | NoDestinationId

type RtmpDestination = {
    id: DestinationId
    serverUrl: string
    streamName: string
} with
    member this.JsonObject = Map.ofList [
        match this.id with
        | CustomDestinationId id -> ("id", id :> obj)
        | NoDestinationId -> ()

        ("serverUrl", this.serverUrl :> obj)
        ("streamName", this.streamName :> obj)
    ]

type BroadcastTargets = {
    hls: bool
    rtmp: RtmpDestination seq
} with
    static member HlsOnly = {
        hls = true
        rtmp = Seq.empty
    }
    static member RtmpOnly serverUrl streamName = {
        hls = false
        rtmp = seq {
            {
                id = NoDestinationId
                serverUrl = serverUrl
                streamName = streamName
            }
        }
    }
    member this.JsonObject = Map.ofList [
        if this.hls then
            ("hls", Map.empty :> obj)
        if not (Seq.isEmpty this.rtmp) then
            ("rtmp", [for r in this.rtmp do yield r.JsonObject] :> obj)
    ]

type BroadcastStartRequest = {
    sessionId: string
    layout: Layout
    maxDuration: TimeSpan
    outputs: BroadcastTargets
    resolution: Resolution
} with
    member this.JsonObject = Map.ofList [
        ("sessionId", this.sessionId :> obj)
        ("layout", this.layout.JsonObject :> obj)
        ("maxDuration", (int this.maxDuration.TotalSeconds) :> obj)
        ("resolution", this.resolution.Dimensions :> obj)
        ("outputs", this.outputs.JsonObject :> obj)
    ]