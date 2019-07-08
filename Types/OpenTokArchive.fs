namespace OpenTokFs.Types

open System

/// <summary>
/// A running or completed OpenTok archive.
/// </summary>
type OpenTokArchive = {
    createdAt: int64
    duration: int
    hasAudio: bool
    hasVideo: bool
    id: string
    name: string
    outputMode: string
    projectId: int
    reason: string
    resolution: string
    sessionId: string
    size: int64
    status: string
    url: string
} with
    member this.GetCreationTime() = DateTimeOffset.FromUnixTimeMilliseconds this.createdAt
    member this.GetDuration() = this.duration |> float |> TimeSpan.FromSeconds