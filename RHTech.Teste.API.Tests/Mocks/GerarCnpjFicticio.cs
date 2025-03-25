using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHTech.Teste.API.Tests.Mocks;

[ExcludeFromCodeCoverage]
public static class GerarCnpjFicticio
{
    public static string CnpjFicticio()
    {
        var random = new Random();
        var cnpj = $"{random.Next(10, 99)}{random.Next(100, 999)}{random.Next(100, 999)}{random.Next(1000, 9999)}{random.Next(10, 99)}";
        return cnpj;
    }

    public static string CpfFicticio()
    {
        var random = new Random();
        var cnpj = $"{random.Next(100, 999)}{random.Next(100, 999)}{random.Next(100, 999)}{random.Next(10, 99)}";
        return cnpj;
    }
}
