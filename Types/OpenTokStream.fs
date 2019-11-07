namespace OpenTokFs.Types

/// An OpenTok stream.
[<AllowNullLiteral>]
type OpenTokStream() =
    /// The stream ID.
    member val Id: string = "" with get, set
    /// The videoType (either "camera" or "screen").
    member val VideoType: string = "" with get, set
    /// The stream name (if one was set when the stream was published).
    member val Name: string = "" with get, set
    /// The array of layout classes for the stream (if any).
    member val LayoutClassList: string[] = Array.empty with get, set