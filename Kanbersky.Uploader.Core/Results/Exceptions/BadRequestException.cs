using Microsoft.AspNetCore.Http;
using System;

namespace Kanbersky.Uploader.Core.Results.Exceptions
{
    public class BadRequestException : Exception, IBaseException
    {
        public int BaseStatusCode { get; set; }

        public BadRequestException()
        {
            BaseStatusCode = StatusCodes.Status400BadRequest;
        }

        public BadRequestException(string message) : base(message)
        {
            BaseStatusCode = StatusCodes.Status400BadRequest;
        }

        public BadRequestException(string message, Exception exception) : base(message, exception)
        {
            BaseStatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
