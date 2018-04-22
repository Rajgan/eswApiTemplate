using System;
using System.Net;
using WebApi.Template.Model.Common;

namespace WebApi.Template.Model.Exceptions
{
    public class InternalServerException : BaseException
    {
        public InternalServerException(HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(ErrorTypeCode.InternalError, statusCode)
        {
        }

        public InternalServerException(Exception inner, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(ErrorTypeCode.InternalError, statusCode, inner)
        {
        }
    }
}
