namespace OpenTokFs.RequestTypes

open System.Runtime.InteropServices

/// <summary>
/// An object that provides information about the video layout to use for an OpenTok archive or broadcast.
/// </summary>
type VideoLayout(``type``: string, [<Optional;DefaultParameterValue(null: string)>] stylesheet: string) =
    member __.Type = ``type``
    member __.Stylesheet = stylesheet

    static member BestFit = new VideoLayout("bestFit")
    static member Pip = new VideoLayout("pip")
    static member VerticalPresentation = new VideoLayout("verticalPresentation")
    static member HorizontalPresentation = new VideoLayout("horizontalPresentation")
    static member Custom (stylesheet: string) = new VideoLayout("custom", stylesheet)