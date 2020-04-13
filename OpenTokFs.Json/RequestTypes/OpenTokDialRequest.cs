using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTokFs.Json.RequestTypes {
    public class OpenTokDialRequest {
        public string SessionId { get; set; }
        public string Token { get; set; }
        public SipInfo Sip { get; set; }

        public class SipInfo {
            public Uri Uri { get; set; }
            public string From { get; set; }
            public Dictionary<string, string> Headers { get; set; }
            public Credentials Credentials { get; set; }
            public bool Secure { get; set; }
        }

        public class Credentials {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
