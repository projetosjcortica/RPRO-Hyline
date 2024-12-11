using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteImpresao.Domain.Exception
{
    public class ValidationException : SystemException
    {
        public List<string> ErrorsMessages = new List<string>();
        public ValidationException(List<string> errorsMessages)
        {
            ErrorsMessages = errorsMessages;
        }

        public ValidationException(string error) 
        {
            ErrorsMessages = new List<string> { error };
        }

    }
}
