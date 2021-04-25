namespace OpenTokFs.RequestDomain

type ArchiveOutputType = Composed of Resolution * Layout | Individual

type ArchiveStartRequest = {
    sessionId: string
    hasAudio: bool
    hasVideo: bool
    name: string option
    outputType: ArchiveOutputType
} with
    member this.JsonObject = Map.ofList [
        ("sessionId", this.sessionId :> obj)
        ("hasAudio", this.hasAudio :> obj)
        ("hasVideo", this.hasVideo :> obj)
        match this.name with
        | Some str -> ("name", str :> obj)
        | None -> ()
        match this.outputType with
        | Individual ->
            ("outputMode", "individual" :> obj)
        | Composed (resolution, layout) ->
            ("outputMode", "composed" :> obj)
            ("resolution", resolution.Dimensions :> obj)
            ("layout", layout.JsonObject :> obj)
    ]