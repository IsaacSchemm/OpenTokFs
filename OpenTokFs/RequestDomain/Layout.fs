namespace OpenTokFs.RequestDomain

type StandardLayout =
| BestFit
| HorizontalPresentation
| VerticalPresentation
| Pip
with
    member this.Name =
        match this with
        | BestFit -> "bestFit"
        | HorizontalPresentation -> "horizontalPresentation"
        | VerticalPresentation -> "verticalPresentation"
        | Pip -> "pip"

type ScreenshareType = ScreenshareType of StandardLayout

type Layout =
| Standard of StandardLayout
| BestFitOr of ScreenshareType
| CustomCss of string
with
    member this.JsonObject = Map.ofList [
        match this with
        | Standard t ->
            ("type", t.Name :> obj)
        | BestFitOr (ScreenshareType s) ->
            ("type", BestFit.Name :> obj)
            ("screenshareType", s.Name :> obj)
        | CustomCss str ->
            ("type", "custom" :> obj)
            ("stylesheet", str :> obj)
    ]