namespace OpenTokFs.RequestDomain

type ArchiveOutputType = ComposedArchive of Resolution * Layout | IndividualArchive

type ArchiveName = CustomArchiveName of string | NoArchiveName
with
    static member IfNotNullOrEmpty x = if System.String.IsNullOrEmpty x then CustomArchiveName x else NoArchiveName
    static member IfNotNullOrWhiteSpace x = if System.String.IsNullOrWhiteSpace x then CustomArchiveName x else NoArchiveName

type ArchiveStartRequest = {
    sessionId: string
    hasAudio: bool
    hasVideo: bool
    name: ArchiveName
    outputType: ArchiveOutputType
} with
    member this.JsonObject = Map.ofList [
        ("sessionId", this.sessionId :> obj)
        ("hasAudio", this.hasAudio :> obj)
        ("hasVideo", this.hasVideo :> obj)
        match this.name with
        | CustomArchiveName str -> ("name", str :> obj)
        | NoArchiveName -> ()
        match this.outputType with
        | IndividualArchive ->
            ("outputMode", "individual" :> obj)
        | ComposedArchive (resolution, layout) ->
            ("outputMode", "composed" :> obj)
            ("resolution", resolution.Dimensions :> obj)
            ("layout", layout.JsonObject :> obj)
    ]