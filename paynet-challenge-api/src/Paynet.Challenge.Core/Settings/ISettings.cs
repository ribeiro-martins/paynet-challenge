namespace Paynet.Challenge.Core.Settings
{
    public interface ISettings 
    {
        string ConnectionString { get; }

        string SenderEmail { get; }

        string SenderEmailPassword { get; }

        string SigninKey { get; }

        string Audience { get; }

        string Issuer { get; }

        int Seconds { get; }

        int ExpirationInMinutes { get; }
    }
}
