# OpenTokFs

This is an unofficial .NET wrapper for the OpenTok REST API.

Although this library is written in F#, it was written primarily to be used
from a C# or VB.NET application. As such, it does not use option types;
strings can be null, and Nullable<T> is used for value types. However, it does
provide F#-friendly methods that return Async<T> and AsyncSeq<T> in addition
to .NET-style methods that return Task<T>.

Namespaces:

* **OpenTokFs.RequestTypes**: Custom types used as parameters in requests to the server. The library builds a corresponding JSON request internally.
* **OpenTokFs.Types**: OpenTok API response types implemented as .NET classes; used for data returned from the server.
* **OpenTokFs.Requests**: Modules (static classes) with functions that wrap OpenTok API methods.

Most functions take a parameter of the type `OpenTokFs.IOpenTokCredentials`.
This interface can be implemented by any object that can provide an OpenTok
API key and secret. `OpenTokFs.OpenTokCredentials` is provided as a sample
implementation.

To generate OpenTok session tokens, you can use the module
`OpenTokFs.OpenTokSessionTokens`, which is based on the implementation in the
official [OpenTok .NET SDK.](https://github.com/opentok/Opentok-.NET-SDK)
