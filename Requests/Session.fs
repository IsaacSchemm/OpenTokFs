namespace OpenTokFs.Requests

open System
open System.Collections.Generic
open System.IO
open System.Threading.Tasks
open Newtonsoft.Json
open OpenTokFs

module Session =
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