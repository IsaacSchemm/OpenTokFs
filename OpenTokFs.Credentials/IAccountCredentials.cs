namespace OpenTokFs.Credentials {
    public interface IAccountCredentials {
        int ApiKey { get; }
        string ApiSecret { get; }
    }
}
