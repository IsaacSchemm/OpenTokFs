namespace OpenTokFs

/// Any object that provides an API key and secret for the OpenTok REST API.
type IOpenTokCredentials =
    abstract member ApiKey: int
    abstract member ApiSecret: string

/// A record that provides an API key and secret for the OpenTok REST API.
type OpenTokCredentials = {
    apiKey: int
    apiSecret: string
} with
    interface IOpenTokCredentials with
        member this.ApiKey = this.apiKey
        member this.ApiSecret = this.apiSecret