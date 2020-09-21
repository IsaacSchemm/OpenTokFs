using Newtonsoft.Json;

namespace OpenTokFs.RequestTypes {
    public class OpenTokRtmpDestination {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("serverUrl")]
        public string ServerUrl { get; set; }

        [JsonProperty("streamName")]
        public string StreamName { get; set; }

        public override string ToString() => $"{ServerUrl}/{StreamName} ({Id ?? "no ID"})";
    }
}
