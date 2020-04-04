namespace OpenTokFs.Types

/// A newly created OpenTok session.
[<AllowNullLiteral>]
type OpenTokSession() =
    /// The session ID.
    member val Session_id: string = "" with get, set
    /// The OpenTok API key.
    member val Project_id: string = "" with get, set
    /// The creation date and time.
    member val Create_dt: string = "Sat Jan 01 12:00:00 PST 2000" with get, set

    override this.ToString() = this.Session_id