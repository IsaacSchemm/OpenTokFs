﻿using Newtonsoft.Json;

namespace OpenTokFs.RequestTypes {
    public class OpenTokSignal {
        [JsonProperty("type")]
        public string Type { get; set; } = "";

        [JsonProperty("data")]
        public string Data { get; set; } = "";
    }
}
