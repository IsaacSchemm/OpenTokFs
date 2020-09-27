namespace OpenTokFs.Credentials {
    public interface IProjectCredentials {
        int ApiKey { get; }
        string ApiSecret { get; }
    }
}
