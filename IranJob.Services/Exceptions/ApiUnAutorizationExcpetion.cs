using IranJob.Services.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IranJob.Services.Exeptions
{
    public class ApiUnAutorizationExcpetion:Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public ApiResultStatusCode ApiStatusCode { get; set; }
        public object AdditionalData { get; set; }

        public ApiUnAutorizationExcpetion(ApiResultStatusCode statusCode, string message, HttpStatusCode httpStatusCode, Exception exception, object additionalData)
           : base(message, exception)
        {
            ApiStatusCode = statusCode;
            HttpStatusCode = httpStatusCode;
            AdditionalData = additionalData;
        }

        public ApiUnAutorizationExcpetion()
            : this(ApiResultStatusCode.ServerError)
        {
        }

        public ApiUnAutorizationExcpetion(ApiResultStatusCode statusCode)
            : this(statusCode, null)
        {
        }

        public ApiUnAutorizationExcpetion(string message)
            : this(ApiResultStatusCode.ServerError, message)
        {
        }

        public ApiUnAutorizationExcpetion(ApiResultStatusCode statusCode, string message)
            : this(statusCode, message, HttpStatusCode.InternalServerError)
        {
        }

        public ApiUnAutorizationExcpetion(string message, object additionalData)
            : this(ApiResultStatusCode.ServerError, message, additionalData)
        {
        }

        public ApiUnAutorizationExcpetion(ApiResultStatusCode statusCode, object additionalData)
            : this(statusCode, null, additionalData)
        {
        }

        public ApiUnAutorizationExcpetion(ApiResultStatusCode statusCode, string message, object additionalData)
            : this(statusCode, message, HttpStatusCode.InternalServerError, additionalData)
        {
        }

        public ApiUnAutorizationExcpetion(ApiResultStatusCode statusCode, string message, HttpStatusCode httpStatusCode)
            : this(statusCode, message, httpStatusCode, null)
        {
        }

        public ApiUnAutorizationExcpetion(ApiResultStatusCode statusCode, string message, HttpStatusCode httpStatusCode, object additionalData)
            : this(statusCode, message, httpStatusCode, null, additionalData)
        {
        }

        public ApiUnAutorizationExcpetion(string message, Exception exception)
            : this(ApiResultStatusCode.ServerError, message, exception)
        {
        }

        public ApiUnAutorizationExcpetion(string message, Exception exception, object additionalData)
            : this(ApiResultStatusCode.ServerError, message, exception, additionalData)
        {
        }

        public ApiUnAutorizationExcpetion(ApiResultStatusCode statusCode, string message, Exception exception)
            : this(statusCode, message, HttpStatusCode.InternalServerError, exception)
        {
        }

        public ApiUnAutorizationExcpetion(ApiResultStatusCode statusCode, string message, Exception exception, object additionalData)
            : this(statusCode, message, HttpStatusCode.InternalServerError, exception, additionalData)
        {
        }

        public ApiUnAutorizationExcpetion(ApiResultStatusCode statusCode, string message, HttpStatusCode httpStatusCode, Exception exception)
            : this(statusCode, message, httpStatusCode, exception, null)
        {
        }
    }
}
