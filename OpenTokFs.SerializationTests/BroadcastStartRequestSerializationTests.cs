using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTokFs;
using OpenTokFs.RequestTypes;
using System;

namespace OpenTokFs.SerializationTests {
    [TestClass]
    public class BroadcastStartRequestSerializationTests {
        [TestMethod]
        public void BroadcastStartRequestTest1() {
            var req1 = new OpenTokBroadcastStartRequest("sessionIdHere") {
                Duration = TimeSpan.FromMinutes(5),
                Hls = true,
                Layout = OpenTokVideoLayout.BestFit,
                Resolution = "640x480"
            };
            string json1 = OpenTokAuthentication.SerializeObject(req1);
            Assert.AreEqual(@"{""sessionId"":""sessionIdHere"",""layout"":{""type"":""bestFit""},""maxDuration"":300,""outputs"":{""hls"":{},""rtmp"":[]},""resolution"":""640x480""}", json1);
        }

        [TestMethod]
        public void BroadcastStartRequestTest2() {
            var req1 = new OpenTokBroadcastStartRequest("sessionIdHere") {
                Duration = TimeSpan.FromMinutes(5),
                Layout = OpenTokVideoLayout.Custom("https://www.example.com/example.css"),
                Resolution = "640x480",
                Rtmp = new OpenTokRtmpDestination[] {
                    new OpenTokRtmpDestination {
                        ServerUrl = "rtmp://media.example.com",
                        StreamName = "demo"
                    }
                }
            };
            string json1 = OpenTokAuthentication.SerializeObject(req1);
            Assert.AreEqual(@"{""sessionId"":""sessionIdHere"",""layout"":{""type"":""custom"",""stylesheet"":""https://www.example.com/example.css""},""maxDuration"":300,""outputs"":{""rtmp"":[{""serverUrl"":""rtmp://media.example.com"",""streamName"":""demo""}]},""resolution"":""640x480""}", json1);
        }
    }
}
