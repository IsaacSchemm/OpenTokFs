using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTokFs.Json.RequestTypes {
    public class OpenTokDialRequest {
        [JsonProperty("sessionId")]
        public string SessionId { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("sip")]
        public SipInfo Sip { get; set; }

        public class SipInfo {
            [JsonProperty("uri")]
            public Uri Uri { get; set; }

            [JsonProperty("from", NullValueHandling = NullValueHandling.Ignore)]
            public string From { get; set; }

            [JsonProperty("headers")]
            public Dictionary<string, string> Headers { get; set; }

            [JsonProperty("auth", NullValueHandling = NullValueHandling.Ignore)]
            public Credentials Auth { get; set; }

            [JsonProperty("secure", NullValueHandling = NullValueHandling.Ignore)]
            public bool Secure { get; set; }
        }

        public class Credentials {
            [JsonProperty("username")]
            public string Username { get; set; }

            [JsonProperty("password")]
            public string Password { get; set; }
        }
    }
}
