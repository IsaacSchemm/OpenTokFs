open OpenTokFs
open OpenTokFs.Api
open OpenTokFs.RequestDomain
open FSharp.Control

let workflow = async {
    let credentials = { projectApiKey = 12345; projectApiSecret = "secret_here" }

    let! session =
        {
            archiveMode = ArchiveMode.Manual
            location = Some (System.Net.IPAddress.Parse "104.18.18.242")
            p2p_preference = P2PPreference.Enabled
        }
        |> Session.AsyncCreate credentials

    do! Async.Sleep 15000

    let! current_archives =
        Archive.AsyncListAll credentials (OpenTokPageSize.Count 1) (OpenTokSessionId.Id session.Session_id)
        |> AsyncSeq.take 1
        |> AsyncSeq.toListAsync
    if List.isEmpty current_archives then
        let! archive =
            {
                sessionId = session.Session_id
                hasAudio = true
                hasVideo = true
                name = Some "My Name Here"
                outputType = Composed (SD, Standard BestFit)
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