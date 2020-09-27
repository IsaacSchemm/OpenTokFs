namespace OpenTokFs.Api

open System
open System.Threading.Tasks
open OpenTokFs
open OpenTokFs.Credentials
open OpenTokFs.RequestTypes
open OpenTokFs.ResponseTypes
open FSharp.Control

module Archive =
    /// Get details on both completed and in-progress archives.
    let AsyncList (credentials: IProjectCredentials) (paging: OpenTokPagingParameters) (sessionId: OpenTokSessionId) = async {
        let query = seq {
            yield sprintf "offset=%d" paging.offset

            match paging.count with
            | OpenTokPageSize.Count c -> yield sprintf "count=%d" c
            | OpenTokPageSize.Default -> ()

            match sessionId with
            | OpenTokSessionId.Id null -> failwithf "A null session ID was provided. To list all archives, pass OpenTokSessionId.Any instead."
            | OpenTokSessionId.Id s -> yield sprintf "sessionId=%s" (Uri.EscapeDataString s)
            | OpenTokSessionId.Any -> ()
        }

        let req = OpenTokAuthentication.BuildProjectLevelRequest credentials "archive" query
        return! OpenTokAuthentication.AsyncReadJson<OpenTokList<OpenTokArchive>> req
    }

    /// Get details on both completed and in-progress archives.
    let ListAsync credentials paging (sessionId: OpenTokSessionId) =
        AsyncList credentials paging sessionId
        |> Async.StartAsTask

    /// Get details on both completed and in-progress archives, making as many requests to the server as necessary.
    let AsyncListAll credentials pageSize sessionId = asyncSeq {
        let mutable paging = { offset = 0; count = pageSize }
        let mutable finished = false
        while not finished do
            let! list = AsyncList credentials paging sessionId
            for item in list.Items do
                yield item
            if paging.offset + Seq.length list.Items >= list.Count then
                finished <- true
            else
                paging <- { offset = paging.offset + Seq.length list.Items; count = paging.count }
    }

    /// Get details on both completed and in-progress archives, making as many requests to the server as necessary.
    let ListAllAsync credentials max sessionId =
        AsyncListAll credentials (OpenTokPageSize.Count 1000) sessionId
        |> AsyncSeq.take max
        |> AsyncSeq.toListAsync
        |> Async.StartAsTask

    /// Get details on both completed and in-progress archives that were started after the given date and time, making as many requests to the server as necessary.
    let ListAllAfterAsync credentials datetime =
        AsyncListAll credentials OpenTokPageSize.Default OpenTokSessionId.Any
        |> AsyncSeq.takeWhile (fun a -> a.GetCreationTime() > datetime)
        |> AsyncSeq.toListAsync
        |> Async.StartAsTask

    /// Start an archive.
    /// A WebException might be thrown if there is an error in the request or if an archive is already running for the given session.
    let AsyncStart (credentials: IProjectCredentials) (body: OpenTokArchiveStartRequest) = async {
        let req = OpenTokAuthentication.BuildProjectLevelRequest credentials "archive" Seq.empty
        req.Method <- "POST"

        do! OpenTokAuthentication.AsyncWriteJson req body
        return! OpenTokAuthentication.AsyncReadJson<OpenTokArchive> req
    }

    /// Start an archive.
    /// A WebException might be thrown if there is an error in the request or if an archive is already running for the given session.
    let StartAsync credentials body =
        AsyncStart credentials body
        |> Async.StartAsTask

    /// Stop an archive.
    let AsyncStop (credentials: IProjectCredentials) (archiveId: string) = async {
        let path = archiveId |> Uri.EscapeDataString |> sprintf "archive/%s/stop"
        let req = OpenTokAuthentication.BuildProjectLevelRequest credentials path Seq.empty
        req.Method <- "POST"

        return! OpenTokAuthentication.AsyncReadJson<OpenTokArchive> req
    }

    /// Stop an archive.
    let StopAsync credentials archiveId =
        AsyncStop credentials archiveId
        |> Async.StartAsTask

    /// Get information about an archive by its ID.
    let AsyncGet (credentials: IProjectCredentials) (archiveId: string) = async {
        let path = archiveId |> Uri.EscapeDataString |> sprintf "archive/%s"
        let req = OpenTokAuthentication.BuildProjectLevelRequest credentials path Seq.empty

        return! OpenTokAuthentication.AsyncReadJson<OpenTokArchive> req
    }

    /// Get information about an archive by its ID.
    let GetAsync credentials archiveId =
        AsyncGet credentials archiveId
        |> Async.StartAsTask

    /// Delete an archive.
    let AsyncDelete (credentials: IProjectCredentials) (archiveId: string) = async {
        let path = archiveId |> Uri.EscapeDataString |> sprintf "archive/%s"
        let req = OpenTokAuthentication.BuildProjectLevelRequest credentials path Seq.empty
        req.Method <- "DELETE"

        use! resp = req.AsyncGetResponse()
        ignore resp
    }

    /// Delete an archive.
    let DeleteAsync credentials archiveId =
        AsyncDelete credentials archiveId
        |> Async.StartAsTask
        :> Task

    /// Change the layout type of an archive while it is being recorded.
    let AsyncSetLayout (credentials: IProjectCredentials) (archiveId: string) (layout: OpenTokVideoLayout) = async {
        let path = archiveId |> Uri.EscapeDataString |> sprintf "archive/%s/layout"
        let req = OpenTokAuthentication.BuildProjectLevelRequest credentials path Seq.empty
        req.Method <- "PUT"

        do! OpenTokAuthentication.AsyncWriteJson req layout
        return! OpenTokAuthentication.AsyncReadJson<OpenTokArchive> req
    }

    /// Change the layout type of an archive while it is being recorded.
    let SetLayoutAsync credentials archiveId layout =
        AsyncSetLayout credentials archiveId layout
        |> Async.StartAsTask