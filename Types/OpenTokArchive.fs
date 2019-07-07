namespace ISchemm.OpenTokFs.Types

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
}