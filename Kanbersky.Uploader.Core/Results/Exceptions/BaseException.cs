using System;

namespace Kanbersky.Uploader.Core.Results.Exceptions
{
    public class BaseException : Exception
    {
        public static UnAuthorizedException UnAuthorizedException()
        {
            return new UnAuthorizedException();
        }

        public static UnAuthorizedException UnAuthorizedException(string message)
        {
            return new UnAuthorizedException(message);
        }

        public static UnAuthorizedException UnAuthorizedException(string message, Exception ex)
        {
            return new UnAuthorizedException(message, ex);
        }

        public static NotFoundException NotFoundException()
        {
            return new NotFoundException();
        }

        public static NotFoundException NotFoundException(string message)
        {
            return new NotFoundException(message);
        }

        public static NotFoundException NotFoundException(string message, Exception ex)
        {
            return new NotFoundException(message, ex);
        }

        public static BadRequestException BadRequestException()
        {
            return new BadRequestException();
        }

        public static BadRequestException BadRequestException(string message)
        {
            return new BadRequestException(message);
        }

        public static BadRequestException BadRequestException(string message, Exception ex)
        {
            return new BadRequestException(message, ex);
        }
    }
}
