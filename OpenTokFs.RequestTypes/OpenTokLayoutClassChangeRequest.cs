using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace OpenTokFs.RequestTypes {
    public class OpenTokLayoutClassChangeRequest {
        [JsonProperty("items")]
        public IEnumerable<ClassChange> Items { get; set; } = Enumerable.Empty<ClassChange>();

        public class ClassChange {
            [JsonProperty("id")]
            public string Id { get; set; } = "";

            [JsonProperty("layoutClassList")]
            public IEnumerable<string> LayoutClassList { get; set; } = Enumerable.Empty<string>();
        }
    }
}
