namespace OpenTokFs.ResponseTypes

open System
open OpenTokFs.Credentials

type OpenTokProjectDetails = {
    id: int
    secret: string
    status: string
    name: string
    environment: string
    createdAt: int64
} with
    interface IProjectCredentials with
        member this.ApiKey = this.id
        member this.ApiSecret = this.secret
    member this.GetCreationTime () = DateTimeOffset.FromUnixTimeMilliseconds this.createdAt
    override this.ToString () = $"{this.id} ({this.name})"