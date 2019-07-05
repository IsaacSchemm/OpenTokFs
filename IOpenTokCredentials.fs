namespace ISchemm.OpenTokFs

type IOpenTokCredentials =
    abstract member ApiKey: int
    abstract member ApiSecret: string