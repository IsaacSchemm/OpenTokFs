namespace OpenTokFs.Api

open OpenTokFs

module Storage =
    type AzureDomain = DefaultAzureDomain | CustomAzureDomain of string

    type AzureStorageConfiguration = {
        accountName: string
        accountKey: string
        container: string
        domain: AzureDomain
    }

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
            let body = {|
                ``type`` = "azure"
                config =
                    {|
                        accountName = config.accountName
                        accountKey = config.accountKey
                        container = config.container
                        domain = match config.domain with CustomAzureDomain d -> d | DefaultAzureDomain -> null
                    |}
                fallback = match fallback with OpenTokFallback -> "opentok" | NoFallback -> "none"
            |}

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