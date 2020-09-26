namespace OpenTokFs.RequestTypes {
    public class OpenTokSessionCreateRequest {
        public bool ArchiveAlways { get; set; } = false;
        public string? IpAddressLocationHint { get; set; } = null;
        public bool P2PEnabled { get; set; } = false;
    }
}
