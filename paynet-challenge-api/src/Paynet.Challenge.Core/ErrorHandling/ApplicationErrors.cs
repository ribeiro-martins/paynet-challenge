using System.Collections.ObjectModel;

namespace Paynet.Challenge.Core.ErrorHandling
{
    public static class ApplicationErrors
    {
        public static readonly IReadOnlyDictionary<string, string> Errors = new ReadOnlyDictionary<string, string>(
            new Dictionary<string, string>()
            {
                { "GenericFieldsValidationError", "35f215db-f091-45fe-94a6-a83670be4c14" },
                { "UserConfirmPasswordMustBeEqual", "80e44ac7-c63c-4efa-9bfc-5727ffd67a4b" },
                { "UserAlreadyExists", "65e8f4af-096a-4314-8aa5-a84ffbc0229c" },
                { "CepNotFound", "ccb514a0-e84b-4be3-b853-8faa17a7a1cd" },
                { "InvalidUserOrPassword", "88d573fa-c875-490f-b48a-990b5a5e7e7c" },
                { "InvalidForgotPasswordCode", "bdb681ca-0ce9-4555-bab3-746ffde066fd" },
            }
        );

        public static KeyValuePair<string, string> GetErrorKeyValuePair(string key)
        {
            Errors.TryGetValue(key, out string value);
            return new KeyValuePair<string, string>(key, value);
        }
    }
}
