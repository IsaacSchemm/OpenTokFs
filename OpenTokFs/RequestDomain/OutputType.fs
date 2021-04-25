namespace OpenTokFs.RequestDomain

type Resolution = SD | HD
with
    static member Default = SD
    member this.Dimensions =
        match this with
        | SD -> "640x480"
        | HD -> "1280x720"

type OutputType = Composed of Resolution * Layout | Individual
with
    static member Default = Composed (Resolution.Default, Layout.Default)