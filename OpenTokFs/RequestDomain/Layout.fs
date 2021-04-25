namespace OpenTokFs.RequestDomain

type LayoutType =
| BestFit
| HorizontalPresentation
| VerticalPresentation
| PIP
with
    static member Default = BestFit
    member this.Name =
        match this with
        | BestFit -> "bestFit"
        | HorizontalPresentation -> "horizontalPresentation"
        | VerticalPresentation -> "verticalPresentation"
        | PIP -> "pip"

type ScreenshareType = ScreenshareType of LayoutType

type Layout =
| BuiltIn of LayoutType
| BestFitOr of ScreenshareType
| CustomCss of string
with
    static member Default = BuiltIn LayoutType.Default
    member this.JsonObject = Map.ofSeq (seq {
        match this with
        | BuiltIn t ->
            ("type", t.Name)
        | BestFitOr (ScreenshareType s) ->
            ("type", BestFit.Name)
            ("screenshareType", s.Name)
        | CustomCss str ->
            ("type", "custom")
            ("stylesheet", str)
    })