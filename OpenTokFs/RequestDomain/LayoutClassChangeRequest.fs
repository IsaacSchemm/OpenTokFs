namespace OpenTokFs.RequestDomain

type LayoutClassChange = {
    id: string
    layoutClassList: string seq
} with
    member this.JsonObject = Map.ofList [
        ("id", this.id :> obj)
        ("layoutClassList", [yield! this.layoutClassList] :> obj)
    ]

type LayoutClassChangeRequest = {
    items: LayoutClassChange seq
} with
   member this.JsonObject = Map.ofList [
       ("items", [for i in this.items do yield i.JsonObject] :> obj)
   ]