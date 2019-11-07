namespace OpenTokFs.Types

open System

/// <summary>
/// A running or completed OpenTok archive.
/// </summary>
[<AllowNullLiteral>]
type OpenTokArchive() =
    member val CreatedAt: int64 = 0L with get, set
    member val Duration: int = 0 with get, set
    member val HasAudio: bool = false with get, set
    member val HasVideo: bool = false with get, set
    member val Id: string = "" with get, set
    member val Name: string = "" with get, set
    member val OutputMode: string = "" with get, set
    member val ProjectId: int = 0 with get, set
    member val Reason: string = "" with get, set
    member val Resolution: string = "" with get, set
    member val SessionId: string = "" with get, set
    member val Size: int64 = 0L with get, set
    member val Status: string = "" with get, set
    member val Url: string = null with get, set

    member this.GetCreationTime() = DateTimeOffset.FromUnixTimeMilliseconds this.CreatedAt
    member this.GetDuration() = this.Duration |> float |> TimeSpan.FromSeconds