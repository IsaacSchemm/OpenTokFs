using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OpenTokFs.RequestTypes;

namespace OpenTokFs.SerializationTests {
    [TestClass]
    public class ArchiveStartRequestSerializationTests {
        [TestMethod]
        public void ArchiveStartRequestTest1() {
            var req1 = new OpenTokArchiveStartRequest("sessionIdHere") {
                HasAudio = true,
                HasVideo = false,
                Layout = OpenTokVideoLayout.BestFit,
                Name = "name-here",
                OutputMode = "composed",
                Resolution = "640x480"
            };
            string json1 = JsonConvert.SerializeObject(req1);
            Assert.AreEqual(@"{""sessionId"":""sessionIdHere"",""hasAudio"":true,""hasVideo"":false,""name"":""name-here"",""outputMode"":""composed"",""layout"":{""type"":""bestFit""},""resolution"":""640x480""}", json1);
        }

        [TestMethod]
        public void ArchiveStartRequestTest2() {
            var req1 = new OpenTokArchiveStartRequest("sessionIdHere") {
                HasAudio = true,
                HasVideo = false,
                Name = "name-here",
                OutputMode = "individual"
            };
            string json1 = JsonConvert.SerializeObject(req1);
            Assert.AreEqual(@"{""sessionId"":""sessionIdHere"",""hasAudio"":true,""hasVideo"":false,""name"":""name-here"",""outputMode"":""individual""}", json1);
        }
    }
}
