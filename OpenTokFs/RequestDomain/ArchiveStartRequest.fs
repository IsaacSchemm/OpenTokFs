namespace OpenTokFs.RequestDomain

type ArchiveOutputType = ComposedArchive of Resolution * Layout | IndividualArchive

type ArchiveNameSetting = ArchiveName of string | NoArchiveName
with
    static member IfNotNullOrEmpty x = if System.String.IsNullOrEmpty x then ArchiveName x else NoArchiveName
    static member IfNotNullOrWhiteSpace x = if System.String.IsNullOrWhiteSpace x then ArchiveName x else NoArchiveName

type ArchiveStartRequest = {
    sessionId: string
    hasAudio: bool
    hasVideo: bool
    name: ArchiveNameSetting
    outputType: ArchiveOutputType
} with
    member this.JsonObject = Map.ofList [
        ("sessionId", this.sessionId :> obj)
        ("hasAudio", this.hasAudio :> obj)
        ("hasVideo", this.hasVideo :> obj)
        match this.name with
        | ArchiveName str -> ("name", str :> obj)
        | NoArchiveName -> ()
        match this.outputType with
        | IndividualArchive ->
            ("outputMode", "individual" :> obj)
        | ComposedArchive (resolution, layout) ->
            ("outputMode", "composed" :> obj)
            ("resolution", resolution.Dimensions :> obj)
            ("layout", layout.JsonObject :> obj)
    ]