using System;

namespace OpenTokFs.Credentials {
    /// <summary>
    /// A set of account-level credentials for Vonage Video API.
    /// You can use <seealso cref="AccountCredentials"/> or implement this interface on your own.
    /// </summary>
    public interface IAccountCredentials {
        /// <summary>
        /// The account-level API key.
        /// </summary>
        int ApiKey { get; }

        /// <summary>
        /// The account-level API secret.
        /// </summary>
        string ApiSecret { get; }
    }

    /// <summary>
    /// A set of account-level credentials for Vonage Video API.
    /// </summary>
    public readonly struct AccountCredentials : IAccountCredentials, IEquatable<AccountCredentials> {
        /// <summary>
        /// The account-level API key.
        /// </summary>
        public int ApiKey { get; }

        /// <summary>
        /// The account-level API secret.
        /// </summary>
        public string ApiSecret { get; }

        public AccountCredentials(int apiKey, string apiSecret) {
            ApiKey = apiKey;
            ApiSecret = apiSecret ?? throw new ArgumentNullException(nameof(apiSecret));
        }

        public override bool Equals(object obj) =>
            obj is AccountCredentials credentials && Equals(credentials);

        public bool Equals(AccountCredentials other) =>
            (ApiKey, ApiSecret) == (other.ApiKey, other.ApiSecret);

        public override int GetHashCode() =>
            (ApiKey, ApiSecret).GetHashCode();

        public static bool operator ==(AccountCredentials left, AccountCredentials right) =>
            left.Equals(right);

        public static bool operator !=(AccountCredentials left, AccountCredentials right) =>
            !(left == right);
    }
}
