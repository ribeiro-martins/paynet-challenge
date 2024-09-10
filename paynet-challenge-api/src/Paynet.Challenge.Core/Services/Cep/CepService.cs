using Newtonsoft.Json.Linq;

namespace Paynet.Challenge.Core.Services.Cep
{
    public class CepService : ICepService
    {
        private readonly HttpClient _httpClient;

        public CepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public bool IsCepValid(string cep)
        {
            cep = cep.Replace("-", "").Trim();

            if (cep.Length != 8 || !long.TryParse(cep, out _))
            {
                return false;
            }

            var response = _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/").Result;

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var content = response.Content.ReadAsStringAsync().Result;
             var jsonResponse = JObject.Parse(content);

            if (jsonResponse["erro"] != null && jsonResponse["erro"].ToString() == "true")
            {
                return false;
            }
            
            return true;
        }
    }
}
