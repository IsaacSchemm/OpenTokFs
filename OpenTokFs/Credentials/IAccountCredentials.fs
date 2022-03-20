namespace OpenTokFs.Credentials

type IAccountCredentials =
    abstract member ApiKey: int
    abstract member ApiSecret: string
