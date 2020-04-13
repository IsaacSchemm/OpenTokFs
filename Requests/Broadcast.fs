namespace OpenTokFs.Requests

open System
open System.IO
open System.Runtime.InteropServices
open Newtonsoft.Json
open OpenTokFs
open OpenTokFs.Json.RequestTypes
open OpenTokFs.Json.ResponseTypes
open OpenTokFs.RequestOptions
open FSharp.Control

module Broadcast =
    /// Get details on broadcasts that are currently in progress. Completed broadcasts are not included.
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
        return! OpenTokAuthentication.AsyncReadJson<OpenTokList<OpenTokBroadcast>> req
    }

    /// Get details on broadcasts that are currently in progress. Completed broadcasts are not included.
    let ListAsync credentials paging ([<Optional;DefaultParameterValue(null)>] sessionId: string) =
        sessionId
        |> Option.ofObj
        |> AsyncList credentials paging
        |> Async.StartAsTask

    /// Get details on broadcasts that are currently in progress, making as many requests to the server as necessary.
    let AsyncListAll credentials sessionId = asyncSeq {
        let mutable paging = { offset = 0; count = Nullable 1000 }
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

    /// Get details on broadcasts that are currently in progress, making as many requests to the server as necessary.
    let ListAllAsync credentials max ([<Optional;DefaultParameterValue(null)>] sessionId: string) =
        sessionId
        |> Option.ofObj
        |> AsyncListAll credentials
        |> AsyncSeq.take max
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    /// Start a broadcast.
    /// A WebException might be thrown if there is an error in the request or if a broadcast is already running for the given session.
    /// (Even if an error is thrown, a broadcast may have been started; use one of the List functions to check.)
    let AsyncStart (credentials: IOpenTokCredentials) (body: BroadcastStartRequest) = async {
        let req = OpenTokAuthentication.BuildRequest credentials "broadcast" Seq.empty
        req.Method <- "POST"
        req.ContentType <- "application/json"

        do! async {
            use! rs = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(rs)
            do! body.AsSerializableObject() |> JsonConvert.SerializeObject |> sw.WriteLineAsync |> Async.AwaitTask
        }

        return! OpenTokAuthentication.AsyncReadJson<OpenTokBroadcast> req
    }

    /// Start a broadcast.
    /// A WebException might be thrown if there is an error in the request or if a broadcast is already running for the given session.
    /// (Even if an error is thrown, a broadcast may have been started; use one of the List functions to check.)
    let StartAsync credentials body =
        AsyncStart credentials body
        |> Async.StartAsTask

    /// Stop a broadcast.
    /// A WebException might be thrown if there is an error in the request or if additonal broadcasts are also running for the session.
    /// (Even if an error is thrown, the broadcast may have been stopped; use one of the List functions to check.)
    let AsyncStop (credentials: IOpenTokCredentials) (broadcastId: string) = async {
        let path = broadcastId |> Uri.EscapeDataString |> sprintf "broadcast/%s/stop"
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty
        req.Method <- "POST"

        return! OpenTokAuthentication.AsyncReadJson<OpenTokBroadcast> req
    }

    /// Stop a broadcast.
    /// A WebException might be thrown if there is an error in the request or if additonal broadcasts are also running for the session.
    /// (Even if an error is thrown, the broadcast may have been stopped; use one of the List functions to check.)
    let StopAsync credentials broadcastId =
        AsyncStop credentials broadcastId
        |> Async.StartAsTask

    /// Get information about a broadcast by its ID.
    let AsyncGet (credentials: IOpenTokCredentials) (broadcastId: string) = async {
        let path = broadcastId |> Uri.EscapeDataString |> sprintf "broadcast/%s"
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty

        return! OpenTokAuthentication.AsyncReadJson<OpenTokBroadcast> req
    }

    /// Get information about a broadcast by its ID.
    let GetAsync credentials broadcastId =
        AsyncGet credentials broadcastId
        |> Async.StartAsTask

    /// Change the layout type of an active broadcast.
    let AsyncSetLayout (credentials: IOpenTokCredentials) (broadcastId: string) (layout: OpenTokVideoLayout) = async {
        let path = broadcastId |> Uri.EscapeDataString |> sprintf "broadcast/%s/layout"
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty
        req.Method <- "PUT"
        req.ContentType <- "application/json"
        
        do! async {
            use! rs = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(rs)
            do! layout |> JsonConvert.SerializeObject |> sw.WriteLineAsync |> Async.AwaitTask
        }

        return! OpenTokAuthentication.AsyncReadJson<OpenTokBroadcast> req
    }

    /// Change the layout type of an active broadcast.
    let SetLayoutAsync credentials broadcastId layout =
        AsyncSetLayout credentials broadcastId layout
        |> Async.StartAsTask