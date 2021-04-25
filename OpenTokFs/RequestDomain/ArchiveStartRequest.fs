namespace OpenTokFs.RequestDomain

type ArchiveName =
| CustomArchiveName of string
| NoArchiveName

type ArchiveStartParameters = {
    hasAudio: bool
    hasVideo: bool
    name: ArchiveName
    outputType: OutputType
} with
    static member Default = {
        hasAudio = true
        hasVideo = true
        name = NoArchiveName
        outputType = OutputType.Default
    }

type ArchiveStartRequest = {
    sessionId: string
    parameters: ArchiveStartParameters
} with
    member this.JsonObject = Map.ofSeq (seq {
        let o x = x :> obj
        ("sessionId", o this.sessionId)
        ("hasAudio", o this.parameters.hasAudio)
        ("hasVideo", o this.parameters.hasVideo)
        match this.parameters.name with
        | CustomArchiveName str -> ("name", o str)
        | NoArchiveName -> ()
        match this.parameters.outputType with
        | Individual ->
            ("outputMode", o "individual")
        | Composed (resolution, layout) ->
            ("outputMode", o "composed")
            ("resolution", o resolution.Dimensions)
            ("layout", o layout.JsonObject)
    })