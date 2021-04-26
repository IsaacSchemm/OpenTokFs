namespace OpenTokFs.Api

open System
open System.Threading.Tasks
open OpenTokFs
open OpenTokFs.Credentials
open OpenTokFs.RequestDomain
open OpenTokFs.ResponseTypes
open FSharp.Control

module Archive =
    /// Get details on both completed and in-progress archives.
    let AsyncList (credentials: IProjectCredentials) (boundaries: PageBoundaries) (filter: SessionIdFilter) = async {
        let query = seq {
            yield! boundaries.QueryString
            yield! filter.QueryString
        }

        let req = OpenTokAuthentication.BuildProjectLevelRequest credentials "archive" query
        return! OpenTokAuthentication.AsyncReadJson<OpenTokList<OpenTokArchive>> req
    }

    /// Get details on both completed and in-progress archives.
    let ListAsync credentials boundaries filter =
        AsyncList credentials boundaries filter
        |> Async.StartAsTask

    /// Get details on both completed and in-progress archives, starting with the given page and continuing as needed.
    let AsyncBeginList credentials first_page filter = asyncSeq {
        let mutable boundaries = first_page
        let mutable finished = false
        while not finished do
            let! list = AsyncList credentials boundaries filter
            for item in list.Items do
                yield item
            if boundaries.offset + Seq.length list.Items >= list.Count then
                finished <- true
            else
                boundaries <- { boundaries with offset = boundaries.offset + Seq.length list.Items }
    }

    /// Get details on both completed and in-progress archives, making as many requests to the server as necessary.
    let AsyncListAll credentials paging filter =
        match paging.limit with
        | StopAtItemCount max ->
            AsyncBeginList credentials paging.first_page filter
            |> Paging.AsyncToList paging.limit
        | StopAtCreationDate datetime ->
            AsyncBeginList credentials paging.first_page filter
            |> AsyncSeq.takeWhile (fun a -> a.GetCreationTime() > datetime)
            |> AsyncSeq.toListAsync
        | NoPageLimit ->
            AsyncBeginList credentials paging.first_page filter
            |> AsyncSeq.toListAsync

    /// Get details on both completed and in-progress archives, making as many requests to the server as necessary.
    let ListAllAsync credentials paging filter =
        AsyncListAll credentials paging filter
        |> Async.StartAsTask

    /// Start an archive.
    /// A WebException might be thrown if there is an error in the request or if an archive is already running for the given session.
    let AsyncStart (credentials: IProjectCredentials) (body: ArchiveStartRequest) = async {
        let req = OpenTokAuthentication.BuildProjectLevelRequest credentials "archive" Seq.empty
        req.Method <- "POST"

        do! OpenTokAuthentication.AsyncWriteJson req body.JsonObject
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
    let AsyncSetLayout (credentials: IProjectCredentials) (archiveId: string) (layout: Layout) = async {
        let path = archiveId |> Uri.EscapeDataString |> sprintf "archive/%s/layout"
        let req = OpenTokAuthentication.BuildProjectLevelRequest credentials path Seq.empty
        req.Method <- "PUT"

        do! OpenTokAuthentication.AsyncWriteJson req layout.JsonObject
        return! OpenTokAuthentication.AsyncReadJson<OpenTokArchive> req
    }

    /// Change the layout type of an archive while it is being recorded.
    let SetLayoutAsync credentials archiveId layout =
        AsyncSetLayout credentials archiveId layout
        |> Async.StartAsTask