namespace OpenTokFs.OptionUtilities

open System

module StringOption =
    let Some x: string option = Some x
    let None: string option = None
    let IfNotNull x: string option = Option.ofObj x
    let IfNotNullOrEmpty x = if String.IsNullOrEmpty x then None else Some x
    let IfNotNullOrWhitespace x = if String.IsNullOrWhiteSpace x then None else Some x