﻿namespace OpenTokFs.Api

open System
open OpenTokFs
open OpenTokFs.Credentials
open OpenTokFs.RequestDomain
open OpenTokFs.ResponseTypes
open FSharp.Control

module Broadcast =
    /// Get details on broadcasts that are currently in progress. Completed broadcasts are not included.
    let AsyncList (credentials: IProjectCredentials) (paging: Paging) (filter: SessionIdFilter) = async {
        let query = seq {
            yield! paging.QueryString
            yield! filter.QueryString
        }

        let req = OpenTokAuthentication.BuildProjectLevelRequest credentials "broadcast" query
        return! OpenTokAuthentication.AsyncReadJson<OpenTokList<OpenTokBroadcast>> req
    }

    /// Get details on broadcasts that are currently in progress. Completed broadcasts are not included.
    let ListAsync credentials paging sessionId =
        AsyncList credentials paging sessionId
        |> Async.StartAsTask

    /// Get details on broadcasts that are currently in progress, making as many requests to the server as necessary.
    let ListAsAsyncSeq credentials initial_paging sessionId = asyncSeq {
        let mutable paging = initial_paging
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

    /// Get details on broadcasts that are currently in progress, asking for 1000 results per page and making as many requests to the server as necessary.
    let AsyncListAll credentials max sessionId =
        ListAsAsyncSeq credentials { offset = 0; count = ExplicitMaximum 1000 } sessionId
        |> AsyncSeq.take max
        |> AsyncSeq.toListAsync

    /// Get details on broadcasts that are currently in progress, asking for 1000 results per page and making as many requests to the server as necessary.
    let ListAllAsync credentials max sessionId =
        AsyncListAll credentials max sessionId
        |> Async.StartAsTask

    /// Start a broadcast.
    /// A WebException might be thrown if there is an error in the request or if a broadcast is already running for the given session.
    /// (Even if an error is thrown, a broadcast may have been started; use one of the List functions to check.)
    let AsyncStart (credentials: IProjectCredentials) (body: BroadcastStartRequest) = async {
        let req = OpenTokAuthentication.BuildProjectLevelRequest credentials "broadcast" Seq.empty
        req.Method <- "POST"

        do! OpenTokAuthentication.AsyncWriteJson req body.JsonObject
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
    let AsyncStop (credentials: IProjectCredentials) (broadcastId: string) = async {
        let path = broadcastId |> Uri.EscapeDataString |> sprintf "broadcast/%s/stop"
        let req = OpenTokAuthentication.BuildProjectLevelRequest credentials path Seq.empty
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
    let AsyncGet (credentials: IProjectCredentials) (broadcastId: string) = async {
        let path = broadcastId |> Uri.EscapeDataString |> sprintf "broadcast/%s"
        let req = OpenTokAuthentication.BuildProjectLevelRequest credentials path Seq.empty

        return! OpenTokAuthentication.AsyncReadJson<OpenTokBroadcast> req
    }

    /// Get information about a broadcast by its ID.
    let GetAsync credentials broadcastId =
        AsyncGet credentials broadcastId
        |> Async.StartAsTask

    /// Change the layout type of an active broadcast.
    let AsyncSetLayout (credentials: IProjectCredentials) (broadcastId: string) (layout: Layout) = async {
        let path = broadcastId |> Uri.EscapeDataString |> sprintf "broadcast/%s/layout"
        let req = OpenTokAuthentication.BuildProjectLevelRequest credentials path Seq.empty
        req.Method <- "PUT"
        
        do! OpenTokAuthentication.AsyncWriteJson req layout.JsonObject
        return! OpenTokAuthentication.AsyncReadJson<OpenTokBroadcast> req
    }

    /// Change the layout type of an active broadcast.
    let SetLayoutAsync credentials broadcastId layout =
        AsyncSetLayout credentials broadcastId layout
        |> Async.StartAsTask