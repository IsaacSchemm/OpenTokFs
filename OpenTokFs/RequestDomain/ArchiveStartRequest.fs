namespace OpenTokFs.RequestDomain

type ArchiveName =
| CustomArchiveName of string
| NoArchiveName

type ArchiveOutputType = Composed of Resolution * Layout | Individual

type ArchiveStartRequest = {
    sessionId: string
    hasAudio: bool
    hasVideo: bool
    name: ArchiveName
    outputType: ArchiveOutputType
} with
    member this.JsonObject = Map.ofList [
        let o x = x :> obj
        ("sessionId", o this.sessionId)
        ("hasAudio", o this.hasAudio)
        ("hasVideo", o this.hasVideo)
        match this.name with
        | CustomArchiveName str -> ("name", o str)
        | NoArchiveName -> ()
        match this.outputType with
        | Individual ->
            ("outputMode", o "individual")
        | Composed (resolution, layout) ->
            ("outputMode", o "composed")
            ("resolution", o resolution.Dimensions)
            ("layout", o layout.JsonObject)
    ]