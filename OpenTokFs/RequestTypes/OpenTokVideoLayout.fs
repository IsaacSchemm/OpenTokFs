namespace OpenTokFs.RequestTypes

open Newtonsoft.Json

type OpenTokVideoLayout() =
    member val ``type``: string = "" with get, set

    [<JsonProperty(NullValueHandling = NullValueHandling.Ignore)>]
    member val stylesheet: string = null with get, set

    static member BestFit = new OpenTokVideoLayout(``type`` = "bestFit")
    static member Pip = new OpenTokVideoLayout(``type`` = "pip")
    static member VerticalPresentation = new OpenTokVideoLayout(``type`` = "verticalPresentation")
    static member HorizontalPresentation = new OpenTokVideoLayout(``type`` = "horizontalPresentation")
    static member Custom css = new OpenTokVideoLayout(``type`` = "custom", stylesheet = css)
