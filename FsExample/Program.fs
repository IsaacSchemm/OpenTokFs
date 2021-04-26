open OpenTokFs.Credentials
open OpenTokFs.Api
open OpenTokFs.RequestDomain
open FSharp.Control

let workflow = async {
    let credentials = {
        new IProjectCredentials with
            member __.ApiKey = 12345
            member __.ApiSecret = "secret_here"
    }

    let! session =
        {
            archiveMode = ManuallyArchive
            location = FirstClientConnectionLocation
            p2p_preference = RoutedSession
        }
        |> Session.AsyncCreate credentials

    do! Async.Sleep 15000

    let paging = {
        first_page = { offset = 0; count = 5 }
        limit = StopAtItemCount 10
    }
    let! previous_archives = Archive.AsyncListAll credentials paging (SingleSessionId session.Session_id)
    printfn "%A" previous_archives

    let! archive =
        {
            sessionId = session.Session_id
            hasAudio = true
            hasVideo = true
            name = CustomArchiveName "My Name Here"
            outputType = ComposedArchive (StandardDefinition, Standard VerticalPresentation)
        }
        |> Archive.AsyncStart credentials

    do! Async.Sleep 10000

    let! updated = Archive.AsyncSetLayout credentials archive.Id (BestFitOr (ScreenshareType Pip))
    ignore updated

    do! Async.Sleep 10000

    let! stopped = Archive.AsyncStop credentials archive.Id
    ignore stopped
}

[<EntryPoint>]
let main _ =
    Async.RunSynchronously workflow
    0