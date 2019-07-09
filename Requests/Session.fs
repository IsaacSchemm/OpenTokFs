﻿namespace OpenTokFs.Requests

open System
open System.Collections.Generic
open System.IO
open System.Net
open System.Threading.Tasks
open Newtonsoft.Json
open OpenTokFs
open OpenTokFs.RequestTypes
open OpenTokFs.Types

module Session =
    /// <summary>
    /// Create a session.
    /// </summary>
    let AsyncCreate (credentials: IOpenTokCredentials) (session: ISessionCreateRequest) = async {
        let req = WebRequest.CreateHttp "https://api.opentok.com/session/create"
        req.Headers.Add("X-OPENTOK-AUTH", OpenTokAuthentication.CreateToken credentials)
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        do! async {
            use! rs = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(rs)

            let parameters = seq {
                yield sprintf "archiveMode=%s" (if session.ArchiveAlways then "always" else "manual")
                yield sprintf "p2p.preference=%s" (if session.BypassMediaRouter then "enabled" else "disabled")
                if not (String.IsNullOrEmpty session.IpAddressLocationHint) then
                    yield sprintf "location=%s" session.IpAddressLocationHint
            }
            do! parameters |> String.concat "&" |> sw.WriteLineAsync |> Async.AwaitTask
        }

        use! resp = req.AsyncGetResponse()

        use s = resp.GetResponseStream()
        use sr = new StreamReader(s)
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask

        return JsonConvert.DeserializeObject<seq<OpenTokSession>> json |> Seq.exactlyOne
    }

    /// <summary>
    /// Create a session.
    /// </summary>
    let CreateAsync credentials session =
        AsyncCreate credentials session
        |> Async.StartAsTask

    /// <summary>
    /// Get information about one stream in a session. A WebException will be thrown if the stream no longer exists.
    /// </summary>
    let AsyncGetStream (credentials: IOpenTokCredentials) (sessionId: string) (streamId: string) = async {
        let path = sprintf "session/%s/stream/%s" (Uri.EscapeDataString sessionId) (Uri.EscapeDataString streamId)
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty

        use! resp = req.AsyncGetResponse()

        use s = resp.GetResponseStream()
        use sr = new StreamReader(s)
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask

        return JsonConvert.DeserializeObject<OpenTokStream> json
    }

    /// <summary>
    /// Get information about one stream in a session. A WebException will be thrown if the stream no longer exists.
    /// </summary>
    let GetStreamAsync credentials sessionId streamId =
        AsyncGetStream credentials sessionId streamId
        |> Async.StartAsTask

    /// <summary>
    /// Get information about all streams in a session.
    /// </summary>
    let AsyncGetStreams (credentials: IOpenTokCredentials) (sessionId: string) = async {
        let path = sessionId |> Uri.EscapeDataString |> sprintf "session/%s/stream"
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty

        use! resp = req.AsyncGetResponse()

        use s = resp.GetResponseStream()
        use sr = new StreamReader(s)
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask

        let list = JsonConvert.DeserializeObject<OpenTokList<OpenTokStream>> json
        return list.items
    }

    /// <summary>
    /// Get information about all streams in a session.
    /// </summary>
    let GetStreamsAsync credentials sessionId =
        AsyncGetStreams credentials sessionId
        |> Async.StartAsTask

    /// <summary>
    /// Change the layout classes of OpenTok streams in a broadcast or archive, by providing stream IDs and lists of classes to apply.
    /// </summary>
    let AsyncSetLayoutClasses (credentials: IOpenTokCredentials) (sessionId: string) (layouts: IDictionary<string, seq<string>>) = async {
        let path = sessionId |> Uri.EscapeDataString |> sprintf "session/%s/stream"
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty
        req.Method <- "PUT"
        req.ContentType <- "application/json"

        do! async {
            let o = new Dictionary<string, obj>()
            o.Add("items", seq {
                for x in layouts do
                    let item = new Dictionary<string, obj>()
                    item.Add("id", x.Key)
                    item.Add("layoutClassList", x.Value)
                    yield item
            })

            use! rs = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(rs)
            let json = o |> JsonConvert.SerializeObject
            do! json |> sw.WriteLineAsync |> Async.AwaitTask
        }

        use! resp = req.AsyncGetResponse()
        ignore resp
    }

    /// <summary>
    /// Change the layout classes of OpenTok streams in a broadcast or archive, by providing stream IDs and lists of classes to apply.
    /// </summary>
    let SetLayoutClassesAsync credentials sessionId layouts =
        AsyncSetLayoutClasses credentials sessionId layouts
        |> Async.StartAsTask
        :> Task