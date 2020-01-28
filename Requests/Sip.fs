namespace OpenTokFs.Requests

open System.IO
open System.Net
open Newtonsoft.Json
open OpenTokFs
open OpenTokFs.Sip

module Sip =
    let AsyncDial (credentials: IOpenTokCredentials) (dial: DialRequest) = async {
        let req =
            credentials.ApiKey
            |> sprintf "https://api.opentok.com/v2/project/%d/dial"
            |> WebRequest.CreateHttp
        req.Headers.Add("X-OPENTOK-AUTH", OpenTokAuthentication.CreateToken credentials)
        req.Accept <- "application/json"
        req.Method <- "POST"
        req.ContentType <- "application/json"

        do! async {
            use! rs = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(rs)
            let m = JsonConvert.SerializeObject dial
            System.Diagnostics.Debug.WriteLine m
            do! JsonConvert.SerializeObject dial |> sw.WriteAsync |> Async.AwaitTask
        }

        use! resp = req.AsyncGetResponse()

        use s = resp.GetResponseStream()
        use sr = new StreamReader(s)
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask

        return JsonConvert.DeserializeObject<DialResponse> json
    }

    let DialAsync credentials req =
        AsyncDial credentials req
        |> Async.StartAsTask