# OpenTokFs

This is an unofficial .NET wrapper for Vonage Video API's REST API.

The structure of the **OpenTokFs** project is as follows:

* **RequestDomain**: Record and union types used to define the possible
  parameters of each request to the REST API. The general idea is to help the
  compiler and IDE let the programmer know what options are available and what
  effects they have. Although the code may take longer to write, ideally it
  will be slightly more readable and much less prone to errors. (Response
  objects are dealt with more loosely: they are translated directly to C#
  classes defined in the `OpenTokFs.ResponseTypes` project.)
* **OpenTokAuthentication**: Performs requests to the REST API and applies the
  appropriate authentication headers.
* **OpenTokSessionTokens**: Generates session tokens; based on the
  implementation in the official [.NET SDK.](https://github.com/opentok/Opentok-.NET-SDK)
* **Api**: Modules that contain the individual functions used to call the REST
  API.

## Authentication

Each function in OpenTokFs.Api takes a credentials object as its first
parameter; these types, `IProjectCredentials` (used for most API calls) and
`IAccountCredentials` (used to create, modify, and delete projects), are
defined in the `OpenTokFs.Credentials` project, as are `ProjectCredentials`
and `AccountCredentials` (which you can use if you don't have a need to
implement these interfaces with your own types).

## Usage

Example code (C#):

    var credentials = new ProjectCredentials(12345, "secret_here");

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