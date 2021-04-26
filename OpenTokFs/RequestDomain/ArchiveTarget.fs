namespace OpenTokFs.RequestDomain

type AzureDomain = DefaultAzureDomain | CustomAzureDomain of string

type AzureStorageConfiguration = {
    accountName: string
    accountKey: string
    container: string
    domain: AzureDomain
} with
    member this.JsonObject = Map.ofList [
        ("accountName", this.accountName :> obj)
        ("accountKey", this.accountKey :> obj)
        ("container", this.container :> obj)
        match this.domain with
        | CustomAzureDomain d -> ("domain", d :> obj)
        | DefaultAzureDomain -> ()
    ]

type S3Endpoint = AmazonS3Endpoint | CustomS3Endpoint of string

type S3StorageConfiguration = {
    accessKey: string
    secretKey: string
    bucket: string
    endpoint: S3Endpoint
} with
    member this.JsonObject = Map.ofList [
        ("accessKey", this.accessKey :> obj)
        ("secretKey", this.secretKey :> obj)
        ("bucket", this.bucket :> obj)
        match this.endpoint with
        | CustomS3Endpoint e -> ("endpoint", e :> obj)
        | AmazonS3Endpoint -> ()
    ]

type ArchiveStorageFallback =
| OpenTokArchiveStorageFallback
| NoArchiveStorageFallback

type ArchiveTarget =
| AzureArchiveTarget of AzureStorageConfiguration * ArchiveStorageFallback
| S3ArchiveTarget of S3StorageConfiguration * ArchiveStorageFallback
| NoArchiveTarget