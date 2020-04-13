namespace OpenTokFs.Json.ResponseTypes {
    public class OpenTokSession {
        public string Session_id { get; set; }
        public string Project_id { get; set; }
        public string Create_dt { get; set; }

        public override string ToString() => Session_id;
    }
}
