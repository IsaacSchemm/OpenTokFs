namespace OpenTokFs.ResponseTypes

type OpenTokSession = {
    session_id: string
    project_id: string
    create_dt: string
} with
    override this.ToString () = this.session_id