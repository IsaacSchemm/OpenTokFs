namespace ISchemm.OpenTokFs.Types

type OpenTokArchive = {
    createdAt: int64
    duration: int
    hasAudio: bool
    hasVideo: bool
    id: string
    name: string
    projectId: int
    reason: string
    sessionId: string
    size: int64
    url: string
}