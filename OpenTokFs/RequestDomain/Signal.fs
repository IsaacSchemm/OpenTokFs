namespace OpenTokFs.RequestDomain

type Signal = {
    ``type``: string
    data: string
} with
    member this.JsonObject = Map.ofList [
        ("type", this.``type`` :> obj)
        ("data", this.data :> obj)
    ]