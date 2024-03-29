using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode)
        {
            StatusCode = statusCode;
            Message = GetDefaultMessageForStatusCode(statusCode);
        }
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }
        public ApiResponse(int statusCode, IEnumerable<string> messages = null)
        {
            StatusCode = statusCode;
            if (messages != null && messages.Any())
            {
                Message = string.Join("\n", messages);
            }
            else GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request",
                401 => "You are not authorized",
                403 => "You are forbidden from doing this",
                404 => "Resource not found",
                500 => "Internal server error",
                _ => null
            };
        }


    }
}