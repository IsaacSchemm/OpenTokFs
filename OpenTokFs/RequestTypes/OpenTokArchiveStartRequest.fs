namespace OpenTokFs.RequestTypes

open Newtonsoft.Json

[<JsonConverter(typeof<OpenTokArchiveStartRequestPropertyConverter>)>]
type OpenTokArchiveStartRequest(sessionId: string) =
    member __.sessionId = sessionId

    member val layout: OpenTokVideoLayout = OpenTokVideoLayout.BestFit with get, set
    member val hasAudio: bool = true with get, set
    member val hasVideo: bool = true with get, set
    member val name: string = null with get, set
    member val outputMode: string = "composed" with get, set
    member val resolution: string = "640x480" with get, set

and OpenTokArchiveStartRequestPropertyConverter() =
    inherit JsonConverter()

    override _.CanConvert t =
        t = typeof<OpenTokArchiveStartRequest>

    override _.ReadJson (reader, objectType, existingValue, serializer) =
        base.ReadJson (reader, objectType, existingValue, serializer)

    override _.WriteJson (writer, value, serializer) =
        match value with
        | :? OpenTokArchiveStartRequest as obj ->
            serializer.Serialize (writer, dict [
                ("sessionId", obj.sessionId :> obj)
                ("hasAudio", obj.hasAudio :> obj)
                ("hasVideo", obj.hasVideo :> obj)
                ("name", obj.name :> obj)
                ("outputMode", obj.outputMode :> obj)
                if obj.outputMode = "composed" then
                    //("layout", obj.layout :> obj)
                    ("resolution", obj.resolution :> obj)
            ])
        | _ ->
            serializer.Serialize (writer, value)
