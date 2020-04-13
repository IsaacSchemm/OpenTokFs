# OpenTokFs

This is an unofficial .NET wrapper for the OpenTok REST API.

The `OpenTokFs.Json` C# library contains .NET representations of (most of) the
JSON objects used for requests and responses in the OpenTok API. The main
`OpenTokFs` project is written in F# and contains the main library logic.

Namespaces:

* **OpenTokFs.RequestOptions**: Custom types used as parameters in requests to the server. The library builds a corresponding JSON request internally.
* **OpenTokFs.Types**: OpenTok API response types implemented as .NET classes; used for data returned from the server.
* **OpenTokFs.Requests**: Modules (static classes) with functions that wrap OpenTok API methods.

Most functions take a parameter of the type `OpenTokFs.IOpenTokCredentials`.
This interface can be implemented by any object that can provide an OpenTok
API key and secret. `OpenTokFs.OpenTokCredentials` is provided as a sample
implementation.

To generate OpenTok session tokens, you can use the module
`OpenTokFs.OpenTokSessionTokens`, which is based on the implementation in the
official [OpenTok .NET SDK.](https://github.com/opentok/Opentok-.NET-SDK)
