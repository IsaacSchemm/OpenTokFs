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
            ("type", t.Name)
        | BestFitOr (ScreenshareType s) ->
            ("type", BestFit.Name)
            ("screenshareType", s.Name)
        | CustomCss str ->
            ("type", "custom")
            ("stylesheet", str)
    ]