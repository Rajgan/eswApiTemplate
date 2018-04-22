using System;
using System.Collections.Generic;
using System.Net;
using WebApi.Template.Model.Common;

namespace WebApi.Template.Model.Exceptions
{
    public class BaseException : Exception
    {
        public ErrorTypeCode ErrorTypeCode { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public List<Error> Errors { get; set; }

        public BaseException(string message) : base(message) { }

        public BaseException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public BaseException(ErrorTypeCode errorCode, HttpStatusCode httpStatusCode)
        {
            Errors = new List<Error>();
            ErrorTypeCode = errorCode;
            HttpStatusCode = httpStatusCode;
        }

        public BaseException(ErrorTypeCode errorCode, HttpStatusCode httpStatusCode, Exception inner) : base(inner?.Message, inner)
        {
            Errors = new List<Error>();
            ErrorTypeCode = errorCode;
            HttpStatusCode = httpStatusCode;
        }
    }
}
