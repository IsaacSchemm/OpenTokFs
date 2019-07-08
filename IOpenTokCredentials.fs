namespace ISchemm.OpenTokFs

/// <summary>
/// Any object that provides an API key and secret for the OpenTok REST API.
/// </summary>
type IOpenTokCredentials =
    abstract member ApiKey: int
    abstract member ApiSecret: string

/// <summary>
/// A record that provides an API key and secret for the OpenTok REST API.
/// </summary>
type OpenTokCredentials = {
    apiKey: int
    apiSecret: string
} with
    interface IOpenTokCredentials with
        member this.ApiKey = this.apiKey
        member this.ApiSecret = this.apiSecret