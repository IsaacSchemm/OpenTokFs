namespace OpenTokFs.RequestDomain

open System

type RtmpDestination = {
    id: string option
    serverUrl: string
    streamName: string
} with
    member this.JsonObject = Map.ofList [
        let o x = x :> obj
        ("serverUrl", o this.serverUrl)
        ("streamName", o this.streamName)

        match this.id with
        | Some id -> ("id", o id)
        | None -> ()
    ]

type BroadcastTargets = {
    hls: bool
    rtmp: RtmpDestination seq
} with
    static member HlsOnly = { hls = true; rtmp = Seq.empty }
    member this.JsonObject = Map.ofList [
        let o x = x :> obj
        if this.hls then
            ("hls", o Map.empty)
        if not (Seq.isEmpty this.rtmp) then
            ("rtmp", o [for r in this.rtmp do yield r.JsonObject])
    ]

type BroadcastStartRequest = {
    sessionId: string
    layout: Layout
    maxDuration: TimeSpan
    outputs: BroadcastTargets
    resolution: Resolution
} with
    member this.JsonObject = Map.ofList [
        let o x = x :> obj
        ("sessionId", o this.sessionId)
        ("layout", o this.layout.JsonObject)
        ("maxDuration", o (int this.maxDuration.TotalSeconds))
        ("resolution", o this.resolution.Dimensions)
        ("outputs", o this.outputs.JsonObject)
    ]