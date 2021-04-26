namespace OpenTokFs.Api

open OpenTokFs
open OpenTokFs.RequestDomain

module Storage =
    let AsyncSet credentials target = async {
        let req = OpenTokAuthentication.BuildProjectLevelRequest credentials "archive/storage" Seq.empty

        match target with
        | AzureArchiveTarget (config, fallback) ->
            let body = Map.ofList [
                ("type", "azure" :> obj)
                ("config", config.JsonObject :> obj)
                match fallback with
                | OpenTokArchiveStorageFallback -> ("fallback", "opentok" :> obj)
                | NoArchiveStorageFallback -> ("fallback", "none" :> obj)
            ]

            req.Method <- "PUT"
            do! OpenTokAuthentication.AsyncWriteJson req body
        | S3ArchiveTarget (config, fallback) ->
            let body = Map.ofList [
                ("type", "s3" :> obj)
                ("config", config.JsonObject :> obj)
                match fallback with
                | OpenTokArchiveStorageFallback -> ("fallback", "opentok" :> obj)
                | NoArchiveStorageFallback -> ("fallback", "none" :> obj)
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