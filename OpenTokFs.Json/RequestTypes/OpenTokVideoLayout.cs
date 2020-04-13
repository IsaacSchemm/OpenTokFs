using Newtonsoft.Json;

namespace OpenTokFs.Json.RequestTypes {
    public class OpenTokVideoLayout {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("stylesheet", NullValueHandling = NullValueHandling.Ignore)]
        public string Stylesheet { get; set; }

        public static OpenTokVideoLayout BestFit => new OpenTokVideoLayout { Type = "bestFit" };
        public static OpenTokVideoLayout Pip => new OpenTokVideoLayout { Type = "pip" };
        public static OpenTokVideoLayout VerticalPresentation => new OpenTokVideoLayout { Type = "verticalPresentation" };
        public static OpenTokVideoLayout HorizontalPresentation => new OpenTokVideoLayout { Type = "horizontalPresentation" };

        public static OpenTokVideoLayout Custom(string css) => new OpenTokVideoLayout { Type = "custom", Stylesheet = css };
    }
}
