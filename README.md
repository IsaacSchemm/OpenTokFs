# OpenTokFs

This is an unofficial .NET wrapper for Vonage Video API's REST API.

The namespace `OpenTokFs.Api`, in the main `OpenTokFs` project, contains
wrapper functions for all REST API functions.

Each function in `OpenTokFs.Api` takes a credentials object as its first
parameter; these types, `IProjectCredentials` (used for most API calls) and
`IAccountCredentials` (used to create, modify, and delete projects), are
defined in the `OpenTokFs.Credentials` project. (You can use the
`OpenTokProjectCredentials` and `OpenTokAccountCredentials` records in
`OpenTokFs` if you don't need to define your own implementations.)

`OpenTokFs.RequestTypes` and `OpenTokFs.ResponseTypes` define C#
representations of JSON request and response bodies, respectively.

To generate OpenTok session tokens, you can use the module
`OpenTokSessionTokens`, which is based on the implementation in the official
[OpenTok .NET SDK.](https://github.com/opentok/Opentok-.NET-SDK)

Example code (C#):

    var credentials = new OpenTokProjectCredentials(12345, "secret_here");

    var all_archives_five_newest = await Api.Archive.ListAsync(
        credentials,
        new OpenTokPagingParameters(0, OpenTokPageSize.NewCount(5)),
        OpenTokSessionId.Any);

    var session = await Api.Session.CreateAsync(credentials, new Api.Session.CreationParameters());

    var archive = await Api.Archive.StartAsync(
        credentials,
        new OpenTokArchiveStartRequest(session.Session_id) {
            OutputMode = "composed"
        });

    var other_archives_same_session = await Api.Archive.ListAsync(
        credentials,
        new OpenTokPagingParameters(0, OpenTokPageSize.Default),
        OpenTokSessionId.NewId(session.Session_id));

    await Api.Archive.StopAsync(credentials, archive.Id);
