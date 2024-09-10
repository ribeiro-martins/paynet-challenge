using Microsoft.Extensions.Configuration;

namespace Paynet.Challenge.Core.Settings
{
	public class AppSettings : ISettings
	{
		static private IConfigurationRoot Content;

		public string ConnectionString => Content[nameof(ConnectionString)];

		public string SigninKey => Content[nameof(SigninKey)];

		public string Audience => Content.GetSection("TokenConfigurations")["Audience"];
		
		public string Issuer => Content.GetSection("TokenConfigurations")["Issuer"];

		public string SenderEmail => Content[nameof(SenderEmail)];

		public string SenderEmailPassword => Content[nameof(SenderEmailPassword)];


		public int Seconds => int.Parse(Content.GetSection("TokenConfigurations")["Seconds"]);

		public int ExpirationInMinutes => int.Parse(Content.GetSection("TokenConfigurations")["ExpirationInMinutes"]);

		static AppSettings()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(AppContext.BaseDirectory)
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables();

			Content = builder.Build();
		}
	}
}
