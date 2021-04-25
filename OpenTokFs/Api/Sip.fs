namespace OpenTokFs.Api

open OpenTokFs
open OpenTokFs.Credentials
open OpenTokFs.RequestDomain
open OpenTokFs.ResponseTypes

module Sip =
    let AsyncDial (credentials: IProjectCredentials) (dial: DialRequest) = async {
        let req = OpenTokAuthentication.BuildProjectLevelRequest credentials "dial" Seq.empty
        req.Method <- "POST"

        do! OpenTokAuthentication.AsyncWriteJson req dial.JsonObject

        return! OpenTokAuthentication.AsyncReadJson<OpenTokSipConnection> req
    }

    let DialAsync credentials req =
        AsyncDial credentials req
        |> Async.StartAsTask