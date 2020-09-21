using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenTokFs.RequestTypes {
    public class OpenTokDialRequest {
        [JsonProperty("sessionId")]
        public string SessionId { get; set; } = "";

        [JsonProperty("token")]
        public string Token { get; set; } = "";

        [JsonProperty("sip")]
        public SipInfo Sip { get; set; } = new SipInfo();

        public class SipInfo {
            [JsonProperty("uri")]
            public string Uri { get; set; } = "";

            [JsonProperty("from", NullValueHandling = NullValueHandling.Ignore)]
            public string From { get; set; } = null;

            [JsonProperty("headers")]
            public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

            [JsonProperty("auth", NullValueHandling = NullValueHandling.Ignore)]
            public Credentials Auth { get; set; } = null;

            [JsonProperty("secure")]
            public bool Secure { get; set; } = false;
        }

        public class Credentials {
            [JsonProperty("username")]
            public string Username { get; set; } = "";

            [JsonProperty("password")]
            public string Password { get; set; } = "";
        }
    }
}
