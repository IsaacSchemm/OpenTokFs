namespace OpenTokFs

open OpenTokFs.Credentials

/// A record that provides an account API key and secret for Vonage Video API.
type OpenTokAccountCredentials = {
    accountApiKey: int
    accountApiSecret: string
} with
    interface IAccountCredentials with
        member this.ApiKey = this.accountApiKey
        member this.ApiSecret = this.accountApiSecret