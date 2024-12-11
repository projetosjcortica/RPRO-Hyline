using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteImpresao.Domain.Result
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; set; }

        public Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None() || !isSuccess && error == Error.None())
            {
                throw new ArgumentException("Invalid arguments:", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
        }
        public static Result Sucess() => new Result(true, Error.None());
        public static Result Failure(Error error) => new Result(false, error);
    }
}
