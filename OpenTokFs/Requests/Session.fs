namespace OpenTokFs.Requests

open System
open System.IO
open System.Net
open System.Threading.Tasks
open OpenTokFs
open OpenTokFs.RequestOptions
open OpenTokFs.RequestTypes
open OpenTokFs.ResponseTypes

module Session =
    /// Create a session.
    let AsyncCreate (credentials: IOpenTokCredentials) (session: SessionCreateRequest) = async {
        let req = WebRequest.CreateHttp "https://api.opentok.com/session/create"
        req.Headers.Add("X-OPENTOK-AUTH", OpenTokAuthentication.CreateToken credentials)
        req.Accept <- "application/json"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        do! async {
            use! rs = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(rs)
            do! session.ToQueryString() |> sw.WriteLineAsync |> Async.AwaitTask
        }
        
        let! list = OpenTokAuthentication.AsyncReadJson<OpenTokSession list> req
        return List.exactlyOne list
    }

    /// Create a session.
    let CreateAsync credentials session =
        AsyncCreate credentials session
        |> Async.StartAsTask

    /// Get information about one stream in a session. A WebException will be thrown if the stream no longer exists.
    let AsyncGetStream (credentials: IOpenTokCredentials) (sessionId: string) (streamId: string) = async {
        let path = sprintf "session/%s/stream/%s" (Uri.EscapeDataString sessionId) (Uri.EscapeDataString streamId)
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty

        return! OpenTokAuthentication.AsyncReadJson<OpenTokStream> req
    }

    /// Get information about one stream in a session. A WebException will be thrown if the stream no longer exists.
    let GetStreamAsync credentials sessionId streamId =
        AsyncGetStream credentials sessionId streamId
        |> Async.StartAsTask

    /// Get information about all streams in a session.
    let AsyncGetStreams (credentials: IOpenTokCredentials) (sessionId: string) = async {
        let path = sprintf "session/%s/stream" (Uri.EscapeDataString sessionId)
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty

        return! OpenTokAuthentication.AsyncReadJson<OpenTokList<OpenTokStream>> req
    }

    /// Get information about all streams in a session.
    let GetStreamsAsync credentials sessionId =
        AsyncGetStreams credentials sessionId
        |> Async.StartAsTask

    /// Change the layout classes of OpenTok streams in a composed archive, by providing stream IDs and lists of classes to apply.
    let AsyncSetLayoutClasses (credentials: IOpenTokCredentials) (sessionId: string) (body: OpenTokLayoutClassChangeRequest) = async {
        let path = sprintf "session/%s/stream" (Uri.EscapeDataString sessionId)
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty
        req.Method <- "PUT"
        
        do! OpenTokAuthentication.AsyncWriteJson req body

        use! resp = req.AsyncGetResponse()
        ignore resp
    }

    /// Change the layout classes of OpenTok streams in a composed archive, by providing stream IDs and lists of classes to apply.
    let SetLayoutClassesAsync credentials sessionId layouts =
        AsyncSetLayoutClasses credentials sessionId layouts
        |> Async.StartAsTask
        :> Task

    /// Send a signal to one participant in a session.
    let AsyncSendSignal (credentials: IOpenTokCredentials) (sessionId: string) (connectionId: string) (signal: OpenTokSignal) = async {
        let path = sprintf "session/%s/connection/%s/signal" (Uri.EscapeDataString sessionId) (Uri.EscapeDataString connectionId)
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty
        req.Method <- "POST"

        do! OpenTokAuthentication.AsyncWriteJson req signal

        use! resp = req.AsyncGetResponse()
        ignore resp
    }

    /// Send a signal to one participant in a session.
    let SendSignalAsync credentials sessionId connectionId signal =
        AsyncSendSignal credentials sessionId connectionId signal
        |> Async.StartAsTask
        :> Task

    /// Send a signal to all participants in a session.
    let AsyncSendSignalToAll (credentials: IOpenTokCredentials) (sessionId: string) (signal: OpenTokSignal) = async {
        let path = sprintf "session/%s/signal" (Uri.EscapeDataString sessionId)
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty
        req.Method <- "POST"

        do! OpenTokAuthentication.AsyncWriteJson req signal

        use! resp = req.AsyncGetResponse()
        ignore resp
    }

    /// Send a signal to all participants in a session.
    let SendSignalToAllAsync credentials sessionId signal =
        AsyncSendSignalToAll credentials sessionId signal
        |> Async.StartAsTask
        :> Task

    /// Force a single participant to disconnect from an OpenTok session.
    let AsyncForceDisconnect (credentials: IOpenTokCredentials) (sessionId: string) (connectionId: string) = async {
        let path = sprintf "session/%s/connection/%s" (Uri.EscapeDataString sessionId) (Uri.EscapeDataString connectionId)
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty
        req.Method <- "DELETE"

        use! resp = req.AsyncGetResponse()
        ignore resp
    }

    /// Force a single participant to disconnect from an OpenTok session.
    let ForceDisconnectAsync credentials sessionId connectionId =
        AsyncForceDisconnect credentials sessionId connectionId
        |> Async.StartAsTask
        :> Task