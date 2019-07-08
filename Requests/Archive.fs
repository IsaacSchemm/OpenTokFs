namespace ISchemm.OpenTokFs.Requests

open System
open System.Collections.Generic
open System.IO
open System.Runtime.InteropServices
open Newtonsoft.Json
open ISchemm.OpenTokFs
open ISchemm.OpenTokFs.Types
open ISchemm.OpenTokFs.RequestTypes
open FSharp.Control

module Archive =
    /// <summary>
    /// Get details on both completed and in-progress archives.
    /// </summary>
    let AsyncList (credentials: IOpenTokCredentials) (paging: OpenTokPagingParameters) (sessionId: string option) = async {
        let query = seq {
            yield paging.Offset |> sprintf "offset=%d"
            if paging.Count.HasValue then
                yield paging.Count.Value |> sprintf "count=%d"
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
    /// Get details on both completed and in-progress archives, making as many requests to the server as necessary.
    /// </summary>
    let AsyncListAll credentials sessionId = asyncSeq {
        let paging = new OpenTokPagingParameters()
        let mutable finished = false
        while not finished do
            let! list = AsyncList credentials paging sessionId
            if Seq.isEmpty list.items then
                finished <- true
            else
                for item in list.items do
                    yield item
                paging.Offset <- paging.Offset + Seq.length list.items
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
    let ListAllAsync credentials max ([<Optional;DefaultParameterValue(null)>] sessionId: string) =
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
    let AsyncStart (credentials: IOpenTokCredentials) (body: IArchiveStartRequest) = async {
        let layout = new Dictionary<string, obj>()
        layout.Add("type", body.LayoutType)
        if body.LayoutType = "custom" then
            layout.Add("stylesheet", body.LayoutStylesheet)

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