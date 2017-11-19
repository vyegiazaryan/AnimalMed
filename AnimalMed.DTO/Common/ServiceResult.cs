using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalMed.DTO
{
    public class ServiceResult<T>
    {
        public bool Succeeded { get; set; }

        public T Data { get; set; }

        public string Error { get; set; }

        public static ServiceResult<T> Success(T data) {
            return new ServiceResult<T>
            {
                Succeeded = true,
                Data = data
            };
        }

        public static ServiceResult<T> Fail(string error)
        {
            return new ServiceResult<T>
            {
                Succeeded = false,
                Error = error
            };
        }

    }
}
