using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteImpresao.Domain.Result
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public T Value { get; }
        public Error Error { get; set; }

        public Result(bool isSuccess, T value, Error error)
        {
            if (isSuccess && error != Error.None() || !IsSuccess && error == Error.None())
            {
                throw new ArgumentException("Invalid arguments:", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
            Value = value;
        }
        public static Result<T> Sucess(T value) => new Result<T>(true, value, Error.None());
        public static Result<T> Failure(Error error) => new Result<T>(false, default, error);
    }
}
