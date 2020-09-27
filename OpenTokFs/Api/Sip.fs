namespace OpenTokFs.Api

open System.Net
open OpenTokFs
open OpenTokFs.RequestTypes
open OpenTokFs.ResponseTypes

module Sip =
    let AsyncDial (credentials: IOpenTokCredentials) (dial: OpenTokDialRequest) = async {
        let req =
            credentials.ApiKey
            |> sprintf "https://api.opentok.com/v2/project/%d/dial"
            |> WebRequest.CreateHttp
        req.Headers.Add("X-OPENTOK-AUTH", OpenTokAuthentication.CreateProjectToken credentials)
        req.Accept <- "application/json"
        req.Method <- "POST"

        do! OpenTokAuthentication.AsyncWriteJson req dial

        return! OpenTokAuthentication.AsyncReadJson<OpenTokSipConnection> req
    }

    let DialAsync credentials req =
        AsyncDial credentials req
        |> Async.StartAsTask