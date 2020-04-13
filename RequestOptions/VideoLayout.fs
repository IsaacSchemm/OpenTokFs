namespace OpenTokFs.RequestOptions

/// <summary>
/// An object that provides information about the video layout to use for an OpenTok archive or broadcast.
/// </summary>
type VideoLayout =
| BestFit
| Pip
| VerticalPresentation
| HorizontalPresentation
| Custom of css: string
with
    member internal this.AsSerializableObject() =
        seq {
            match this with
            | BestFit ->
                yield ("type", "bestFit")
            | Pip ->
                yield ("type", "pip")
            | VerticalPresentation ->
                yield ("type", "verticalPresentation")
            | HorizontalPresentation ->
                yield ("type", "horizontalPresentation")
            | Custom css ->
                yield ("type", "custom")
                yield ("stylesheet", css)
        }
        |> dict