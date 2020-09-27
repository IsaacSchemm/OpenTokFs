namespace OpenTokFs

open OpenTokFs.Credentials

/// A record that provides a project API key and secret for Vonage Video API.
type OpenTokProjectCredentials = {
    projectApiKey: int
    projectApiSecret: string
} with
    interface IProjectCredentials with
        member this.ApiKey = this.projectApiKey
        member this.ApiSecret = this.projectApiSecret