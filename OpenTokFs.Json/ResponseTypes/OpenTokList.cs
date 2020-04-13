using System.Collections.Generic;

namespace OpenTokFs.Json.ResponseTypes {
    public class OpenTokList<T> {
        public int Count { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
