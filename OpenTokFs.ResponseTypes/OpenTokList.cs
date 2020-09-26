using System.Collections.Generic;
using System.Linq;

namespace OpenTokFs.ResponseTypes {
    public class OpenTokList<T> {
        public int Count { get; set; } = 0;
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    }
}
