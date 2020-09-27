namespace OpenTokFs.Api

open System.IO
open System.Net
open OpenTokFs
open OpenTokFs.ResponseTypes
open System.Runtime.InteropServices

module Project =
    let AsyncCreate (credentials: IOpenTokAccountCredentials) (name: string option) = async {
        let req = WebRequest.CreateHttp "https://api.opentok.com/v2/project"
        req.Method <- "POST"
        req.Headers.Add("X-OPENTOK-AUTH", OpenTokAuthentication.CreateAccountToken credentials)
        req.Accept <- "application/json"

        match name with
        | None -> ()
        | Some n ->
            req.ContentType <- "application/json"
            use! s = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(s)
            let body = {| name = n |}
            do! body |> OpenTokAuthentication.SerializeObject |> sw.WriteAsync |> Async.AwaitTask

        return! OpenTokAuthentication.AsyncReadJson<OpenTokProjectDetails> req
    }

    let CreateAsync credentials ([<Optional; DefaultParameterValue(null)>] name) =
        AsyncCreate credentials (Option.ofObj name)
        |> Async.StartAsTask

    [<RequireQualifiedAccess>]
    type ProjectStatus = Active | Suspended

    let AsyncChangeProjectStatus (credentials: IOpenTokAccountCredentials) (projectId: int) (status: ProjectStatus) = async {
        let req = WebRequest.CreateHttp (sprintf "https://api.opentok.com/v2/project/%d" projectId)
        req.Method <- "PUT"
        req.Headers.Add("X-OPENTOK-AUTH", OpenTokAuthentication.CreateAccountToken credentials)
        req.ContentType <- "application/json"

        do! async {
            use! s = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(s)
            let body =
                match status with
                | ProjectStatus.Active -> {| status = "ACTIVE" |}
                | ProjectStatus.Suspended -> {| status = "SUSPENDED" |}
            do! body |> OpenTokAuthentication.SerializeObject |> sw.WriteAsync |> Async.AwaitTask
        }

        use! resp = req.AsyncGetResponse()
        ignore resp
    }

    let ChangeProjectStatusAsync credentials projectId status =
        AsyncChangeProjectStatus credentials projectId status
        |> Async.StartAsTask

    let AsyncDeleteProject (credentials: IOpenTokAccountCredentials) (projectId: int) = async {
        let req = WebRequest.CreateHttp (sprintf "https://api.opentok.com/v2/project/%d" projectId)
        req.Method <- "DELETE"
        req.Headers.Add("X-OPENTOK-AUTH", OpenTokAuthentication.CreateAccountToken credentials)

        use! resp = req.AsyncGetResponse()
        ignore resp
    }

    let DeleteProjectAsync credentials projectId =
        AsyncDeleteProject credentials projectId
        |> Async.StartAsTask

    let AsyncGetProject (credentials: IOpenTokAccountCredentials) (projectId: int) = async {
        let req = WebRequest.CreateHttp (sprintf "https://api.opentok.com/v2/project/%d" projectId)
        req.Method <- "GET"
        req.Headers.Add("X-OPENTOK-AUTH", OpenTokAuthentication.CreateAccountToken credentials)
        req.Accept <- "application/json"

        return! OpenTokAuthentication.AsyncReadJson<OpenTokProjectDetails> req
    }

    let GetProjectAsync credentials projectId =
        AsyncGetProject credentials projectId
        |> Async.StartAsTask

    let AsyncGetProjects (credentials: IOpenTokAccountCredentials) = async {
        let req = WebRequest.CreateHttp "https://api.opentok.com/v2/project"
        req.Method <- "GET"
        req.Headers.Add("X-OPENTOK-AUTH", OpenTokAuthentication.CreateAccountToken credentials)
        req.Accept <- "application/json"

        return! OpenTokAuthentication.AsyncReadJson<OpenTokProjectDetails list> req
    }

    let GetProjectsAsync credentials =
        AsyncGetProjects credentials
        |> Async.StartAsTask