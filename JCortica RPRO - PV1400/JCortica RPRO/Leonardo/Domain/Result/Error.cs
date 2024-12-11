using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteImpresao.Domain.Result
{
    public class Error
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public Error(string code, string description)
        {
            Code = code;
            Description = description;
        }
        public static Error None()

        {
            return new Error(string.Empty, string.Empty);
        }

        public static bool operator ==(Error firstError, Error secondError)
        {
            return firstError.Code == secondError.Code && firstError.Description == secondError.Description;
        }

        public static bool operator !=(Error firstError, Error secondError)
        {
            return !(firstError.Code == secondError.Code && firstError.Description == secondError.Description);
        }

        public override string ToString()
        {
            return $"{Code}:{Description}";
        }
    }
}
