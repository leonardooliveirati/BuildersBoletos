using BuildersBoleto.Domain.Models;

namespace BuildersBoleto.Domain.Intefaces.Services
{
    public interface IBoletoService
    {
        Task<BoletoResponse> CalcularValorBoletoAsync(string barCode, DateTime paymentDate, string token);
    }
}