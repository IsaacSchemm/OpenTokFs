namespace OpenTokFs.ResponseTypes

open System

type OpenTokArchive = {
    createdAt: int64
    duration: int
    hasAudio: bool
    hasVideo: bool
    id: string
    name: string // or null
    outputMode: string // or null
    projectId: int
    reason: string
    resolution: string // or null
    sessionId: string
    size: int64
    status: string
    url: string // or null
} with
    member this.GetCreationTime () = DateTimeOffset.FromUnixTimeMilliseconds this.createdAt
    member this.GetDuration () = TimeSpan.FromSeconds this.duration
    override this.ToString () = String.concat " " [
        this.id
        if not (isNull this.name) then $"({this.name})"
        $"({this.outputMode}, {this.status})"
    ]