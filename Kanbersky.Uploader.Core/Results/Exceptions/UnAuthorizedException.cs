using Microsoft.AspNetCore.Http;
using System;

namespace Kanbersky.Uploader.Core.Results.Exceptions
{
    public class UnAuthorizedException : Exception, IBaseException
    {
        public int BaseStatusCode { get; set; }

        public UnAuthorizedException()
        {
            BaseStatusCode = StatusCodes.Status401Unauthorized;
        }

        public UnAuthorizedException(string message) : base(message)
        {
            BaseStatusCode = StatusCodes.Status401Unauthorized;
        }

        public UnAuthorizedException(string message, Exception exception) : base(message, exception)
        {
            BaseStatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}
