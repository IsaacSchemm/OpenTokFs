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

    var session = await Session.CreateAsync(credentials, new NewSession(
        archiveMode: ArchiveMode.ManuallyArchive,
        location: SessionLocation.FirstClientConnectionLocation,
        p2p_preference: MediaMode.RoutedSession));

    await Task.Delay(15000);

    var previous_archives = await Archive.ListAllAsync(
        credentials,
        5,
        SessionIdFilter.NewSingleSessionId(session.Session_id));
    Console.WriteLine(previous_archives);

    var archive = await Archive.StartAsync(
        credentials,
        new ArchiveStartRequest(
            sessionId: session.Session_id,
            hasAudio: true,
            hasVideo: true,
            name: ArchiveNameSetting.NewArchiveName("My Name Here"),
            outputType: ArchiveOutputType.NewComposedArchive(
                Resolution.StandardDefinition,
                Layout.NewLayoutType(LayoutType.VerticalPresentation))));

    await Task.Delay(10000);

    await Archive.SetLayoutAsync(credentials,
        archive.Id,
        Layout.NewBestFitOr(ScreenshareType.NewScreenshareType(LayoutType.Pip)));

    await Task.Delay(10000);

    await Archive.StopAsync(credentials, archive.Id);

Example (F#):

    let credentials = { projectApiKey = 12345; projectApiSecret = "secret_here" }

    let! session =
        {
            archiveMode = ManuallyArchive
            location = FirstClientConnectionLocation
            p2p_preference = RoutedSession
        }
        |> Session.AsyncCreate credentials

    do! Async.Sleep 15000

    let! previous_archives = Archive.AsyncListAll credentials 5 (SingleSessionId session.Session_id)
    printfn "%A" previous_archives

    let! archive =
        {
            sessionId = session.Session_id
            hasAudio = true
            hasVideo = true
            name = ArchiveName "My Name Here"
            outputType = ComposedArchive (StandardDefinition, LayoutType VerticalPresentation)
        }
        |> Archive.AsyncStart credentials

    do! Async.Sleep 10000

    let! updated = Archive.AsyncSetLayout credentials archive.Id (BestFitOr (ScreenshareType Pip))
    ignore updated

    do! Async.Sleep 10000

    let! stopped = Archive.AsyncStop credentials archive.Id
    ignore stopped

See the ArchiveHelper and BroadcastHelper projects for example usage in VB.NET.