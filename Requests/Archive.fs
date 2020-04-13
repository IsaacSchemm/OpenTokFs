namespace OpenTokFs.Requests

open System
open System.Collections.Generic
open System.IO
open System.Runtime.InteropServices
open System.Threading.Tasks
open Newtonsoft.Json
open OpenTokFs
open OpenTokFs.Json.ResponseTypes
open OpenTokFs.RequestTypes
open FSharp.Control

module Archive =
    /// Get details on both completed and in-progress archives.
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
        let layout = body.Layout.ToIDictionary()

        let requestObject =
            seq {
                let o x = x :> obj

                yield ("sessionId", o body.SessionId)
                yield ("hasAudio", o body.HasAudio)
                yield ("hasVideo", o body.HasVideo)
                yield ("name", o body.Name)
                yield ("outputMode", o body.OutputMode)
                if body.OutputMode <> "individual" then
                    yield ("layout", o layout)
                    yield ("resolution", o body.Resolution)
            }
            |> dict

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

    /// Start a broadcast.
    /// A WebException might be thrown if there is an error in the request or if an archive is already running for the given session.
    let StartAsync credentials body =
        AsyncStart credentials body
        |> Async.StartAsTask

    /// Stop an archive.
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

    /// Stop an archive.
    let StopAsync credentials archiveId =
        AsyncStop credentials archiveId
        |> Async.StartAsTask

    /// Get information about an archive by its ID.
    let AsyncGet (credentials: IOpenTokCredentials) (archiveId: string) = async {
        let path = archiveId |> Uri.EscapeDataString |> sprintf "archive/%s"
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty

        use! resp = req.AsyncGetResponse()

        use s = resp.GetResponseStream()
        use sr = new StreamReader(s)
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask

        return JsonConvert.DeserializeObject<OpenTokArchive> json
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
    let AsyncSetLayout (credentials: IOpenTokCredentials) (archiveId: string) (layout: VideoLayout) = async {
        let path = archiveId |> Uri.EscapeDataString |> sprintf "archive/%s/layout"
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty
        req.Method <- "PUT"
        req.ContentType <- "application/json"

        do! async {
            let o = layout.ToIDictionary()

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

    /// Change the layout type of an archive while it is being recorded.
    let SetLayoutAsync credentials archiveId layout =
        AsyncSetLayout credentials archiveId layout
        |> Async.StartAsTask