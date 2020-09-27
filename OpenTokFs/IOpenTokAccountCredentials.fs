namespace OpenTokFs

/// An object that provides an account API key and secret for Vonage Video API.
type IOpenTokAccountCredentials =
    abstract member ApiKey: int
    abstract member ApiSecret: string

/// A record that provides an account API key and secret for Vonage Video API.
type OpenTokAccountCredentials = {
    accountApiKey: int
    accountApiSecret: string
} with
    interface IOpenTokAccountCredentials with
        member this.ApiKey = this.accountApiKey
        member this.ApiSecret = this.accountApiSecret