namespace OpenTokFs

/// An object that provides a project API key and secret for Vonage Video API.
type IOpenTokCredentials =
    abstract member ApiKey: int
    abstract member ApiSecret: string

/// A record that provides a project API key and secret for Vonage Video API.
type OpenTokCredentials = {
    apiKey: int
    apiSecret: string
} with
    interface IOpenTokCredentials with
        member this.ApiKey = this.apiKey
        member this.ApiSecret = this.apiSecret