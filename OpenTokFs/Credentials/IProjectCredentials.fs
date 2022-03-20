namespace OpenTokFs.Credentials

type IProjectCredentials =
    abstract member ApiKey: int
    abstract member ApiSecret: string
