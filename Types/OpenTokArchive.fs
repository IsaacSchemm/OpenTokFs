namespace OpenTokFs.Types

open System

/// A running or completed OpenTok archive.
[<AllowNullLiteral>]
type OpenTokArchive() =
    /// The creation time of the archive (in milliseconds since Jan 1 1970 00:00:00 UTC).
    member val CreatedAt: int64 = 0L with get, set

    /// The duration of the archive (in seconds).
    member val Duration: int = 0 with get, set

    /// Whether the archive has audio.
    member val HasAudio: bool = false with get, set

    /// Whether the archive has video.
    member val HasVideo: bool = false with get, set

    /// The unique archive ID.
    member val Id: string = "" with get, set

    /// The name of the archive, if any.
    member val Name: string = null with get, set

    /// The output mode of the archive (composed or individual).
    member val OutputMode: string = "" with get, set

    /// The OpenTok API key.
    member val ProjectId: int = 0 with get, set

    /// The reason an archive's status is "stopped" or "failed", if any.
    member val Reason: string = "" with get, set

    /// The resolution of the archive.
    member val Resolution: string = "" with get, set

    /// The session ID of the OpenTok session that was archived.
    member val SessionId: string = "" with get, set

    /// The size of the archive file. For archives that have not been generated, this value is set to 0.
    member val Size: int64 = 0L with get, set

    /// The status of the archive (e.g. "available", "expired", "failed")
    member val Status: string = "" with get, set

    /// The download URL of the archive file, if the status is "available".
    member val Url: string = null with get, set

    /// Gets the creation time of the archive as a DateTimeOffset object.
    member this.GetCreationTime() = DateTimeOffset.FromUnixTimeMilliseconds this.CreatedAt

    /// Gets the duration time of the archive as a TimeSpan object.
    member this.GetDuration() = this.Duration |> float |> TimeSpan.FromSeconds

    override this.ToString() = sprintf "%s (%s) (%s, %s)" this.Id this.Name this.OutputMode this.Status