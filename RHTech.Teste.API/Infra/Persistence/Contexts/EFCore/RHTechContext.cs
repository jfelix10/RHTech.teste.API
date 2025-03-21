using Microsoft.EntityFrameworkCore;

namespace RHTech.Teste.API.Infra.Persistence.Contexts.EFCore
{
    public class RHTechContext : DbContext
    {
        public RHTechContext(DbContextOptions<RHTechContext> options) : base(options) { }
    }
}
