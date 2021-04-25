namespace OpenTokFs.Api

open OpenTokFs
open OpenTokFs.Credentials
open OpenTokFs.ResponseTypes
open System.Runtime.InteropServices

module Project =
    let AsyncCreate (credentials: IAccountCredentials) (name: string option) = async {
        let req =
            "https://api.opentok.com/v2/project"
            |> OpenTokAuthentication.BuildAccountLevelRequest credentials
        req.Method <- "POST"
        req.Accept <- "application/json"

        match name with
        | Some n ->
            let map = Map.ofList [("name", n :> obj)]
            do! OpenTokAuthentication.AsyncWriteJson req map
        | None -> ()

        return! OpenTokAuthentication.AsyncReadJson<OpenTokProjectDetails> req
    }

    let CreateAsync credentials ([<Optional; DefaultParameterValue(null)>] name) =
        AsyncCreate credentials (Option.ofObj name)
        |> Async.StartAsTask

    [<RequireQualifiedAccess>]
    type ProjectStatus = Active | Suspended

    let AsyncChangeStatus (credentials: IAccountCredentials) (projectId: int) (status: ProjectStatus) = async {
        let req =
            sprintf "https://api.opentok.com/v2/project/%d" projectId
            |> OpenTokAuthentication.BuildAccountLevelRequest credentials
        req.Method <- "PUT"
        req.ContentType <- "application/json"

        let map = Map.ofList [
            match status with
            | ProjectStatus.Active -> ("status", "ACTIVE" :> obj)
            | ProjectStatus.Suspended -> ("status", "SUSPENDED" :> obj)
        ]
        do! OpenTokAuthentication.AsyncWriteJson req map

        use! resp = req.AsyncGetResponse()
        ignore resp
    }

    let ChangeStatusAsync credentials projectId status =
        AsyncChangeStatus credentials projectId status
        |> Async.StartAsTask

    let AsyncDelete (credentials: IAccountCredentials) (projectId: int) = async {
        let req =
            sprintf "https://api.opentok.com/v2/project/%d" projectId
            |> OpenTokAuthentication.BuildAccountLevelRequest credentials
        req.Method <- "DELETE"

        use! resp = req.AsyncGetResponse()
        ignore resp
    }

    let DeleteAsync credentials projectId =
        AsyncDelete credentials projectId
        |> Async.StartAsTask

    let AsyncGet (credentials: IAccountCredentials) (projectId: int) = async {
        let req =
            sprintf "https://api.opentok.com/v2/project/%d" projectId
            |> OpenTokAuthentication.BuildAccountLevelRequest credentials
        req.Method <- "GET"
        req.Accept <- "application/json"

        return! OpenTokAuthentication.AsyncReadJson<OpenTokProjectDetails> req
    }

    let GetAsync credentials projectId =
        AsyncGet credentials projectId
        |> Async.StartAsTask

    let AsyncListAll (credentials: IAccountCredentials) = async {
        let req =
            "https://api.opentok.com/v2/project"
            |> OpenTokAuthentication.BuildAccountLevelRequest credentials
        req.Method <- "GET"
        req.Accept <- "application/json"

        return! OpenTokAuthentication.AsyncReadJson<OpenTokProjectDetails list> req
    }

    let ListAllAsync credentials =
        AsyncListAll credentials
        |> Async.StartAsTask

    let AsyncRefreshSecret (credentials: IAccountCredentials) (projectId: int) = async {
        let req =
            sprintf "https://api.opentok.com/v2/project/%d/refreshSecret" projectId
            |> OpenTokAuthentication.BuildAccountLevelRequest credentials
        req.Method <- "POST"
        req.Accept <- "application/json"

        return! OpenTokAuthentication.AsyncReadJson<OpenTokProjectDetails> req
    }

    let RefreshSecretAsync credentials projectId =
        AsyncRefreshSecret credentials projectId
        |> Async.StartAsTask