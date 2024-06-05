using BuildersBoleto.Domain.Entities;

namespace BuildersBoleto.Domain.Intefaces.Proxy
{
    public interface IBoletoApiClient
    {
        Task<BoletoModel> GetBoletoAsync(string code, string token);
    }
}