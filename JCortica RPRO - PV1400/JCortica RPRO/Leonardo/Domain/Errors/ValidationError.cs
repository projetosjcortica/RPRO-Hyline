using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteImpresao.Domain.Result;

namespace TesteImpresao.Domain.Errors
{
    public static class ValidationError
    {
        public static Error NotEmpty(string campoName) => new Error("ValidationError", $"O campo \"{campoName}\" valor não pode ser vazio!");
    }
}
