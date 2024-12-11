using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteImpresao.Domain.Result;

namespace TesteImpresao.Domain.Errors
{
    public static class ConnectionError
    {
        public static Error InvalidIPAdress() => new Error("ConnectionError", "Endereço de IP inválido!");
    }
}
