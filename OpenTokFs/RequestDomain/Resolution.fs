namespace OpenTokFs.RequestDomain

type Resolution = StandardDefinition | HighDefinition
with
    member this.Dimensions =
        match this with
        | StandardDefinition -> "640x480"
        | HighDefinition -> "1280x720"