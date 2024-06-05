using BuildersBoleto.Domain.Entities;
using BuildersBoleto.Domain.Intefaces.Repositories;
using BuildersBoleto.Infrastructure.Data;

namespace BuildersBoleto.Infrastructure.Repositories
{
    public class BoletoRepository : IBoletoRepository
    {
        private readonly BuildersBoletoDbContext _context;

        public BoletoRepository(BuildersBoletoDbContext context)
        {
            _context = context;
        }

        public async Task Salvar(BoletoEntity boleto)
        {
            _context.Boletos.Add(boleto);
            await _context.SaveChangesAsync();
        }
    }
}