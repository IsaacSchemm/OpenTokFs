namespace OpenTokFs.RequestDomain

type Resolution = SD | HD
with
    member this.Dimensions =
        match this with
        | SD -> "640x480"
        | HD -> "1280x720"