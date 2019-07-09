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

module Broadcast =
    /// <summary>
    /// Get details on broadcasts that are currently in progress. Completed broadcasts are not included.
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

        let req = OpenTokAuthentication.BuildRequest credentials "broadcast" query
        use! resp = req.AsyncGetResponse()

        use s = resp.GetResponseStream()
        use sr = new StreamReader(s)
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask

        return JsonConvert.DeserializeObject<OpenTokList<OpenTokBroadcast>> json
    }

    /// <summary>
    /// Get details on broadcasts that are currently in progress. Completed broadcasts are not included.
    /// </summary>
    let ListAsync credentials paging ([<Optional;DefaultParameterValue(null)>] sessionId: string) =
        sessionId
        |> Option.ofObj
        |> AsyncList credentials paging
        |> Async.StartAsTask

    /// <summary>
    /// Get details on broadcasts that are currently in progress, making as many requests to the server as necessary.
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
    /// Get details on broadcasts that are currently in progress, making as many requests to the server as necessary.
    /// </summary>
    let ListAllAsync credentials ([<Optional;DefaultParameterValue(Int32.MaxValue)>] max) ([<Optional;DefaultParameterValue(null)>] sessionId: string) =
        sessionId
        |> Option.ofObj
        |> AsyncListAll credentials
        |> AsyncSeq.take max
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    /// <summary>
    /// Start a broadcast.
    /// A WebException might be thrown if there is an error in the request or if a broadcast is already running for the given session.
    /// (Even if an error is thrown, a broadcast may have been started; use one of the List functions to check.)
    /// </summary>
    let AsyncStart (credentials: IOpenTokCredentials) (body: IBroadcastStartRequest) = async {
        let rtmp = seq {
            for r in body.Rtmp do
                let x = new Dictionary<string, obj>()
                if not (String.IsNullOrEmpty r.Id) then
                    x.Add("id", r.Id)
                x.Add("serverUrl", r.ServerUrl)
                x.Add("streamName", r.StreamName)
                yield x
        }

        let layout = new Dictionary<string, obj>()
        layout.Add("type", body.LayoutType)
        if body.LayoutType = "custom" then
            layout.Add("stylesheet", body.LayoutStylesheet)

        let outputs = new Dictionary<string, obj>()
        if body.Hls then
            outputs.Add("hls", new obj())
        outputs.Add("rtmp", rtmp :> obj)

        let requestObject = new Dictionary<string, obj>()
        requestObject.Add("sessionId", body.SessionId)
        requestObject.Add("layout", layout)
        requestObject.Add("maxDuration", body.Duration.TotalSeconds |> int)
        requestObject.Add("outputs", outputs)
        requestObject.Add("resolution", body.Resolution)

        let req = OpenTokAuthentication.BuildRequest credentials "broadcast" Seq.empty
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

        return JsonConvert.DeserializeObject<OpenTokBroadcast> json
    }

    /// <summary>
    /// Start a broadcast.
    /// A WebException might be thrown if there is an error in the request or if a broadcast is already running for the given session.
    /// (Even if an error is thrown, a broadcast may have been started; use one of the List functions to check.)
    /// </summary>
    let StartAsync credentials body =
        AsyncStart credentials body
        |> Async.StartAsTask

    /// <summary>
    /// Stop a broadcast.
    /// A WebException might be thrown if there is an error in the request or if additonal broadcasts are also running for the session.
    /// (Even if an error is thrown, the broadcast may have been stopped; use one of the List functions to check.)
    /// </summary>
    let AsyncStop (credentials: IOpenTokCredentials) (broadcastId: string) = async {
        let path = broadcastId |> Uri.EscapeDataString |> sprintf "broadcast/%s/stop"
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty
        req.Method <- "POST"

        use! resp = req.AsyncGetResponse()

        use s = resp.GetResponseStream()
        use sr = new StreamReader(s)
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask

        return JsonConvert.DeserializeObject<OpenTokBroadcast> json
    }

    /// <summary>
    /// Stop a broadcast.
    /// A WebException might be thrown if there is an error in the request or if additonal broadcasts are also running for the session.
    /// (Even if an error is thrown, the broadcast may have been stopped; use one of the List functions to check.)
    /// </summary>
    let StopAsync credentials broadcastId =
        AsyncStop credentials broadcastId
        |> Async.StartAsTask

    let AsyncGet (credentials: IOpenTokCredentials) (archiveId: string) = async {
        let path = archiveId |> Uri.EscapeDataString |> sprintf "broadcast/%s"
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty

        use! resp = req.AsyncGetResponse()

        use s = resp.GetResponseStream()
        use sr = new StreamReader(s)
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask

        return JsonConvert.DeserializeObject<OpenTokBroadcast> json
    }

    let GetAsync credentials archiveId =
        AsyncGet credentials archiveId
        |> Async.StartAsTask