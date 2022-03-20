namespace OpenTokFs.RequestTypes

type OpenTokRtmpDestination() =
    member val id: string = null with get, set
    member val serverUrl = "" with get, set
    member val streamName = "" with get, set
    override this.ToString () = String.concat " " [
        $"{this.serverUrl}/{this.streamName}"
        if not (isNull this.id) then $"({this.id})"
    ]
