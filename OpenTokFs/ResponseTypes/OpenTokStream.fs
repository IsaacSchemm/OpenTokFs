namespace OpenTokFs.ResponseTypes

open System

type OpenTokStream = {
    id: string
    videoType: string
    name: string // or null
    layoutClassList: string list
} with
    member this.IsCamera = this.videoType = "camera"
    member this.IsScreen = this.videoType = "screen"
    override this.ToString () = String.concat " " [
        this.id
        if not (isNull this.name) then this.name
        this.videoType
    ]