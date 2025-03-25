using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace RHTech.Teste.API.Infra.Persistence.Contexts.EFCore;

[ExcludeFromCodeCoverage]
public class RHTechContext : DbContext
{
    public RHTechContext(DbContextOptions<RHTechContext> options) : base(options) { }
}
