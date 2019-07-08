# OpenTokFs

This is an unofficial .NET wrapper for the OpenTok REST API.

Although this library is written in F#, it is designed first and foremost to
be used from C# or VB.NET. As such, it does not use option types; strings can
be null, and Nullable<T> is used for value types.

Namespaces:

* **OpenTokFs.RequestTypes**: Custom types used as parameters in requests to the server. The library builds a corresponding JSON request internally.
* **OpenTokFs.Types**: OpenTok API response types implemented as F# records; used for data returned from the server.
* **OpenTokFs.Requests**: Static classes with functions that wrap OpenTok API methods.

Most functions take a parameter of the type `OpenTokFs.IOpenTokCredentials`.
This interface can be implemented by any object that can provide an OpenTok
API key and secret. `OpenTokFs.OpenTokCredentials` is provided as a sample
implementation.
