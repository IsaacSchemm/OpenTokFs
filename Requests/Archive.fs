namespace OpenTokFs.Requests

open System
open System.Collections.Generic
open System.IO
open System.Runtime.InteropServices
open Newtonsoft.Json
open OpenTokFs
open OpenTokFs.Types
open OpenTokFs.RequestTypes
open FSharp.Control

module Archive =
    /// <summary>
    /// Get details on both completed and in-progress archives.
    /// </summary>
    let AsyncList (credentials: IOpenTokCredentials) (paging: OpenTokPagingParameters) (sessionId: string option) = async {
        let query = seq {
            yield paging.offset |> sprintf "offset=%d"
            if paging.count.HasValue then
                yield paging.count.Value |> sprintf "count=%d"
            match sessionId with
            | Some s -> yield s |> Uri.EscapeDataString |> sprintf "sessionId=%s"
            | None -> ()
        }

        let req = OpenTokAuthentication.BuildRequest credentials "archive" query
        use! resp = req.AsyncGetResponse()

        use s = resp.GetResponseStream()
        use sr = new StreamReader(s)
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask

        return JsonConvert.DeserializeObject<OpenTokList<OpenTokArchive>> json
    }

    /// <summary>
    /// Get details on both completed and in-progress archives.
    /// </summary>
    let ListAsync credentials paging ([<Optional;DefaultParameterValue(null)>] sessionId: string) =
        sessionId
        |> Option.ofObj
        |> AsyncList credentials paging
        |> Async.StartAsTask

    /// <summary>
    /// Get details on both completed and in-progress archives, making as many requests to the server as necessary.
    /// </summary>
    let AsyncListAll credentials sessionId = asyncSeq {
        let mutable paging = { offset = 0; count = Nullable() }
        let mutable finished = false
        while not finished do
            let! list = AsyncList credentials paging sessionId
            if Seq.isEmpty list.items then
                finished <- true
            else
                for item in list.items do
                    yield item
                paging <- { offset = paging.offset + Seq.length list.items; count = paging.count }
    }

    /// <summary>
    /// Get details on both completed and in-progress archives, making as many requests to the server as necessary.
    /// </summary>
    let ListAllAsync credentials ([<Optional;DefaultParameterValue(Int32.MaxValue)>] max) ([<Optional;DefaultParameterValue(null)>] sessionId: string) =
        sessionId
        |> Option.ofObj
        |> AsyncListAll credentials
        |> AsyncSeq.take max
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    /// <summary>
    /// Start an archive.
    /// A WebException might be thrown if there is an error in the request or if an archive is already running for the given session.
    /// </summary>
    let AsyncStart (credentials: IOpenTokCredentials) (body: ArchiveStartRequest) = async {
        let layout = body.Layout.ToSerializableObject()

        let requestObject = new Dictionary<string, obj>()
        requestObject.Add("sessionId", body.SessionId)
        requestObject.Add("hasAudio", body.HasAudio)
        requestObject.Add("hasVideo", body.HasVideo)
        requestObject.Add("layout", layout)
        requestObject.Add("name", body.Name)
        requestObject.Add("outputMode", body.OutputMode)
        requestObject.Add("resolution", body.Resolution)

        let req = OpenTokAuthentication.BuildRequest credentials "archive" Seq.empty
        req.Method <- "POST"
        req.ContentType <- "application/json"

        do! async {
            use! rs = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(rs)
            do! requestObject |> JsonConvert.SerializeObject |> sw.WriteLineAsync |> Async.AwaitTask
        }

        use! resp = req.AsyncGetResponse()

        use s = resp.GetResponseStream()
        use sr = new StreamReader(s)
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask

        return JsonConvert.DeserializeObject<OpenTokArchive> json
    }

    /// <summary>
    /// Start a broadcast.
    /// A WebException might be thrown if there is an error in the request or if an archive is already running for the given session.
    /// </summary>
    let StartAsync credentials body =
        AsyncStart credentials body
        |> Async.StartAsTask

    /// <summary>
    /// Stop an archive.
    /// </summary>
    let AsyncStop (credentials: IOpenTokCredentials) (archiveId: string) = async {
        let path = archiveId |> Uri.EscapeDataString |> sprintf "archive/%s/stop"
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty
        req.Method <- "POST"

        use! resp = req.AsyncGetResponse()

        use s = resp.GetResponseStream()
        use sr = new StreamReader(s)
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask

        return JsonConvert.DeserializeObject<OpenTokArchive> json
    }

    /// <summary>
    /// Stop an archive.
    /// </summary>
    let StopAsync credentials archiveId =
        AsyncStop credentials archiveId
        |> Async.StartAsTask

    /// <summary>
    /// Get information about an archive by its ID.
    /// </summary>
    let AsyncGet (credentials: IOpenTokCredentials) (archiveId: string) = async {
        let path = archiveId |> Uri.EscapeDataString |> sprintf "archive/%s"
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty

        use! resp = req.AsyncGetResponse()

        use s = resp.GetResponseStream()
        use sr = new StreamReader(s)
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask

        return JsonConvert.DeserializeObject<OpenTokArchive> json
    }

    /// <summary>
    /// Get information about an archive by its ID.
    /// </summary>
    let GetAsync credentials archiveId =
        AsyncGet credentials archiveId
        |> Async.StartAsTask

    /// <summary>
    /// Change the layout type of an archive while it is being recorded.
    /// </summary>
    let AsyncSetLayout (credentials: IOpenTokCredentials) (archiveId: string) (layout: VideoLayout) = async {
        let path = archiveId |> Uri.EscapeDataString |> sprintf "archive/%s/layout"
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty
        req.Method <- "PUT"
        req.ContentType <- "application/json"

        do! async {
            let o = layout.ToSerializableObject()

            use! rs = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(rs)
            do! o |> JsonConvert.SerializeObject |> sw.WriteLineAsync |> Async.AwaitTask
        }

        use! resp = req.AsyncGetResponse()

        use s = resp.GetResponseStream()
        use sr = new StreamReader(s)
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask

        return JsonConvert.DeserializeObject<OpenTokArchive> json
    }

    /// <summary>
    /// Change the layout type of an archive while it is being recorded.
    /// </summary>
    let SetLayoutAsync credentials archiveId layout =
        AsyncSetLayout credentials archiveId layout
        |> Async.StartAsTask