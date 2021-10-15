using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Momkn.Core.DTOs;

namespace Momkn.API.Helpers
{
    public class ResponseHelper
    {
        public static ResponseModel Success(dynamic data = null, string message= "Operation is done successfully.")
        {
           

            return new ResponseModel { Success = true, StatusCode = 200, Message = message, Data = data };
        }

        public static ResponseModel Fail(string message = "Something went wrong, try again." )
        {
            return new ResponseModel { Success = false, StatusCode = 403, Message = message };
        }
    }
}
