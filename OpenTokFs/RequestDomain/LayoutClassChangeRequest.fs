namespace OpenTokFs.RequestDomain

type LayoutClassChangeItem = {
    id: string
    layoutClassList: string seq
} with
    member this.JsonObject = Map.ofList [
        ("id", this.id :> obj)
        ("layoutClassList", [yield! this.layoutClassList] :> obj)
    ]

type LayoutClassChangeRequest = {
    items: LayoutClassChangeItem seq
} with
   member this.JsonObject = Map.ofList [
       ("items", [for i in this.items do yield i.JsonObject] :> obj)
   ]