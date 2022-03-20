namespace OpenTokFs.RequestTypes

open System
open Newtonsoft.Json

[<JsonConverter(typeof<OpenTokBroadcastStartRequestPropertyConverter>)>]
type OpenTokBroadcastStartRequest(sessionId: string) =
    member __.sessionId = sessionId

    member val layout: OpenTokVideoLayout = OpenTokVideoLayout.BestFit with get, set
    member val duration: TimeSpan = TimeSpan.FromHours 2 with get, set
    member val Hls: bool = false with get, set
    member val Rtmp: OpenTokRtmpDestination seq = Seq.empty with get, set
    member val resolution: string = "640x480" with get, set

and OpenTokBroadcastStartRequestPropertyConverter() =
    inherit JsonConverter()

    override _.CanConvert t =
        t = typeof<OpenTokBroadcastStartRequest>

    override _.ReadJson (reader, objectType, existingValue, serializer) =
        base.ReadJson (reader, objectType, existingValue, serializer)

    override _.WriteJson (writer, value, serializer) =
        match value with
        | :? OpenTokBroadcastStartRequest as obj ->
            serializer.Serialize (writer, dict [
                ("sessionId", obj.sessionId :> obj)
                ("layout", obj.layout :> obj)
                ("maxDuration", obj.duration.TotalSeconds :> obj)
                ("outputs", [
                    if obj.Hls then ("hls", {||} :> obj)
                    ("rtmp", obj.Rtmp :> obj)
                ] :> obj)
                ("resolution", obj.resolution :> obj)
            ])
        | _ ->
            serializer.Serialize (writer, value)
