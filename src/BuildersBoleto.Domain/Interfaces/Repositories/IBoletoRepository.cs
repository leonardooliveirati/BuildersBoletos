using BuildersBoleto.Domain.Entities;

namespace BuildersBoleto.Domain.Intefaces.Repositories
{
    public interface IBoletoRepository
    {
        Task Salvar(BoletoEntity boleto);
    }
}