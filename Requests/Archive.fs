﻿namespace OpenTokFs.Requests

open System
open System.Runtime.InteropServices
open System.Threading.Tasks
open OpenTokFs
open OpenTokFs.Json.RequestTypes
open OpenTokFs.Json.ResponseTypes
open OpenTokFs.RequestOptions
open FSharp.Control

module Archive =
    /// Get details on both completed and in-progress archives.
    let AsyncList (credentials: IOpenTokCredentials) (paging: OpenTokPagingParameters) (sessionId: string option) = async {
        let query = seq {
            yield sprintf "offset=%d" paging.offset

            match Option.ofNullable paging.count with
            | Some c -> yield sprintf "count=%d" c
            | None -> ()

            match sessionId with
            | Some s -> yield sprintf "sessionId=%s" (Uri.EscapeDataString s)
            | None -> ()
        }

        let req = OpenTokAuthentication.BuildRequest credentials "archive" query
        return! OpenTokAuthentication.AsyncReadJson<OpenTokList<OpenTokArchive>> req
    }

    /// Get details on both completed and in-progress archives.
    let ListAsync credentials paging ([<Optional;DefaultParameterValue(null)>] sessionId: string) =
        sessionId
        |> Option.ofObj
        |> AsyncList credentials paging
        |> Async.StartAsTask

    /// Get details on both completed and in-progress archives, making as many requests to the server as necessary.
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

    /// Get details on both completed and in-progress archives, making as many requests to the server as necessary.
    let ListAllAsync credentials max ([<Optional;DefaultParameterValue(null)>] sessionId: string) =
        sessionId
        |> Option.ofObj
        |> AsyncListAll credentials
        |> AsyncSeq.take max
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    /// Start an archive.
    /// A WebException might be thrown if there is an error in the request or if an archive is already running for the given session.
    let AsyncStart (credentials: IOpenTokCredentials) (body: ArchiveStartRequest) = async {
        let req = OpenTokAuthentication.BuildRequest credentials "archive" Seq.empty
        req.Method <- "POST"

        do! OpenTokAuthentication.AsyncWriteJson req (body.AsSerializableObject())
        return! OpenTokAuthentication.AsyncReadJson<OpenTokArchive> req
    }

    /// Start an archive.
    /// A WebException might be thrown if there is an error in the request or if an archive is already running for the given session.
    let StartAsync credentials body =
        AsyncStart credentials body
        |> Async.StartAsTask

    /// Stop an archive.
    let AsyncStop (credentials: IOpenTokCredentials) (archiveId: string) = async {
        let path = archiveId |> Uri.EscapeDataString |> sprintf "archive/%s/stop"
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty
        req.Method <- "POST"

        return! OpenTokAuthentication.AsyncReadJson<OpenTokArchive> req
    }

    /// Stop an archive.
    let StopAsync credentials archiveId =
        AsyncStop credentials archiveId
        |> Async.StartAsTask

    /// Get information about an archive by its ID.
    let AsyncGet (credentials: IOpenTokCredentials) (archiveId: string) = async {
        let path = archiveId |> Uri.EscapeDataString |> sprintf "archive/%s"
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty

        return! OpenTokAuthentication.AsyncReadJson<OpenTokArchive> req
    }

    /// Get information about an archive by its ID.
    let GetAsync credentials archiveId =
        AsyncGet credentials archiveId
        |> Async.StartAsTask

    /// Delete an archive.
    let AsyncDelete (credentials: IOpenTokCredentials) (archiveId: string) = async {
        let path = archiveId |> Uri.EscapeDataString |> sprintf "archive/%s"
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty
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
    let AsyncSetLayout (credentials: IOpenTokCredentials) (archiveId: string) (layout: OpenTokVideoLayout) = async {
        let path = archiveId |> Uri.EscapeDataString |> sprintf "archive/%s/layout"
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty
        req.Method <- "PUT"

        do! OpenTokAuthentication.AsyncWriteJson req layout
        return! OpenTokAuthentication.AsyncReadJson<OpenTokArchive> req
    }

    /// Change the layout type of an archive while it is being recorded.
    let SetLayoutAsync credentials archiveId layout =
        AsyncSetLayout credentials archiveId layout
        |> Async.StartAsTask