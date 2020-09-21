using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTokFs;
using OpenTokFs.RequestTypes;

namespace OpenTokFs.SerializationTests {
    [TestClass]
    public class DialRequestSerializationTests {
        [TestMethod]
        public void DialRequestSerializationTest1() {
            var req = new OpenTokDialRequest {
                SessionId = "session-id-here",
                Token = "token-here",
                Sip = new OpenTokDialRequest.SipInfo {
                    Uri = "sip:user@sipexample.com;transport=tls",
                    From = "from@example.com",
                    Headers = new System.Collections.Generic.Dictionary<string, string> {
                        ["X-Example-1"] = "val-1",
                        ["X-Example-2"] = "val-2"
                    },
                    Auth = new OpenTokDialRequest.Credentials {
                        Username = "username-here",
                        Password = "password-here"
                    },
                    Secure = true
                }
            };
            string json = OpenTokAuthentication.SerializeObject(req);
            Assert.AreEqual(@"{""sessionId"":""session-id-here"",""token"":""token-here"",""sip"":{""uri"":""sip:user@sipexample.com;transport=tls"",""from"":""from@example.com"",""headers"":{""X-Example-1"":""val-1"",""X-Example-2"":""val-2""},""auth"":{""username"":""username-here"",""password"":""password-here""},""secure"":true}}", json);
        }

        [TestMethod]
        public void DialRequestSerializationTest2() {
            var req = new OpenTokDialRequest {
                SessionId = "session-id-here",
                Token = "token-here",
                Sip = new OpenTokDialRequest.SipInfo {
                    Uri = "sip:user@sipexample.com;transport=tls"
                }
            };
            string json = OpenTokAuthentication.SerializeObject(req);
            Assert.AreEqual(@"{""sessionId"":""session-id-here"",""token"":""token-here"",""sip"":{""uri"":""sip:user@sipexample.com;transport=tls"",""headers"":{},""secure"":false}}", json);
        }
    }
}
