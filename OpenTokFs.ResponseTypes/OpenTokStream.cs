using System.Collections.Generic;
using System.Linq;

namespace OpenTokFs.ResponseTypes {
    public class OpenTokStream {
        public string Id { get; set; } = "";
        public string VideoType { get; set; } = "";
        public string? Name { get; set; } = null;
        public IEnumerable<string> LayoutClassList { get; set; } = Enumerable.Empty<string>();

        public bool IsCamera => VideoType == "camera";
        public bool IsScreen => VideoType == "screen";

        public override string ToString() => $"{Id} ({Name ?? "no name"}) ({VideoType})";
    }
}
