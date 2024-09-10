namespace Paynet.Challenge.Core.Services.Cep
{
    public interface ICepService
    {
        bool IsCepValid(string cep);
    }
}