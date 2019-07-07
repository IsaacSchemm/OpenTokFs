﻿namespace ISchemm.OpenTokFs.Requests

open System
open System.Collections.Generic
open System.IO
open System.Runtime.InteropServices
open Newtonsoft.Json
open ISchemm.OpenTokFs
open ISchemm.OpenTokFs.Types
open ISchemm.OpenTokFs.RequestTypes

module Broadcast =
    /// <summary>
    /// Get details on broadcasts that are currently in progress. Completed broadcasts are not included.
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
    /// Start a broadcast. A WebException will be thrown if there is an error in the request or if a broadcast is already running for the given session.
    /// </summary>
    let AsyncStart (credentials: IOpenTokCredentials) (body: IBroadcastStartRequest) = async {
        match (body.LayoutType, String.IsNullOrEmpty body.LayoutStylesheet) with
        | ("custom", false) -> ()
        | ("custom", true) -> failwithf "The 'custom' layout type requires a stylesheet."
        | (_, false) -> failwithf "Stylesheets can only be used with the 'custom' layout type."
        | (_, true) -> ()

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
    /// A WebException will be thrown if there is an error in the request or if a broadcast is already running for the given session.
    /// (Even if an error is thrown, a broadcast may have been started; use one of the List functions to check.)
    /// </summary>
    let StartAsync credentials body =
        AsyncStart credentials body
        |> Async.StartAsTask

    /// <summary>
    /// Stop a broadcast.
    /// A WebException will be thrown if there is an error in the request or if additonal broadcasts are also running for the session.
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
    /// A WebException will be thrown if there is an error in the request or if additonal broadcasts are also running for the session.
    /// (Even if an error is thrown, the broadcast may have been stopped; use one of the List functions to check.)
    /// </summary>
    let StopAsync credentials broadcastId =
        AsyncStop credentials broadcastId
        |> Async.StartAsTask