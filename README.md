# OpenTokFs

This is an unofficial .NET wrapper for Vonage Video API's REST API.

The namespace `OpenTokFs.Api`, in the main `OpenTokFs` project, contains
wrapper functions for the REST API. Request parameters use F# record and union
types; the idea is to help the compiler and IDE inform the programmer of what
options are available and what effects they have. Response objects are dealt
with more loosely: they are translated directly to C# classes defined in the
`OpenTokFs.ResponseTypes` project.

## Authentication

Each function in `OpenTokFs.Api` takes a credentials object as its first
parameter; these types, `IProjectCredentials` (used for most API calls) and
`IAccountCredentials` (used to create, modify, and delete projects), are
defined in the `OpenTokFs.Credentials` project. (You can use the
`OpenTokProjectCredentials` and `OpenTokAccountCredentials` records in
`OpenTokFs` if you don't need to define your own implementations.)

To generate OpenTok session tokens, you can use the module
`OpenTokSessionTokens`, which is based on the implementation in the official
[OpenTok .NET SDK.](https://github.com/opentok/Opentok-.NET-SDK)

## Usage

Example code (C#):

    var credentials = new OpenTokProjectCredentials(12345, "secret_here");

    var session = await Session.CreateAsync(credentials, new SessionCreationParameters(
        archiveMode: ArchiveMode.Manual,
        location: Option.Some(System.Net.IPAddress.Parse("104.18.18.242")),
        p2p_preference: P2PPreference.Enabled));

    await Task.Delay(15000);

    var current_archives = await Archive.ListAllAsync(
        credentials,
        1,
        OpenTokSessionId.NewId(session.Session_id));
    if (current_archives.IsEmpty) {
        var archive = await Archive.StartAsync(
            credentials,
            new ArchiveStartRequest(
                sessionId: session.Session_id,
                hasAudio: true,
                hasVideo: true,
                name: Option.Some("My Name Here"),
                outputType: ArchiveOutputType.NewComposed(
                    Resolution.SD,
                    Layout.NewStandard(StandardLayout.BestFit))));

        await Task.Delay(10000);

        await Archive.SetLayoutAsync(credentials,
            archive.Id,
            Layout.NewBestFitOr(ScreenshareType.NewScreenshareType(StandardLayout.Pip)));

        await Task.Delay(10000);

        await Archive.StopAsync(credentials, archive.Id);

Example (F#):

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