using System.Net;
using WebApi.Template.Model.Common;

namespace WebApi.Template.Model.Exceptions
{
    public class ValidationException : BaseException
    {
        public ValidationException(HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(ErrorTypeCode.ValidationError, statusCode)
        {
        }

        public ValidationException(string message) : base(message)
        {

        }
    }
}
