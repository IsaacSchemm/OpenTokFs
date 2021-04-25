namespace OpenTokFs.Api

open OpenTokFs

module Storage =
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

    type Fallback =
    | OpenTokFallback
    | NoFallback

    type ArchiveTarget =
    | AzureArchiveTarget of AzureStorageConfiguration * Fallback
    | NoArchiveTarget

    let AsyncSet credentials target = async {
        let req = OpenTokAuthentication.BuildProjectLevelRequest credentials "archive/storage" Seq.empty

        match target with
        | AzureArchiveTarget (config, fallback) ->
            let body = Map.ofList [
                ("type", "azure" :> obj)
                ("config", config.JsonObject :> obj)
                match fallback with
                | OpenTokFallback -> ("fallback", "opentok" :> obj)
                | NoFallback -> ("fallback", "none" :> obj)
            ]

            req.Method <- "PUT"
            do! OpenTokAuthentication.AsyncWriteJson req body
        | NoArchiveTarget ->
            req.Method <- "DELETE"

        use! resp = req.AsyncGetResponse()
        ignore resp
    }

    let SetAsync credentials target =
        AsyncSet credentials target
        |> Async.StartAsTask