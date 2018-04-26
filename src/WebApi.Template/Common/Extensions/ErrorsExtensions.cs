using System.Collections.Generic;
using WebApi.Template.Model.Exceptions;

namespace WebApi.Template.Common.Extensions
{
    /// <summary>
    /// Extension Method to add Errors
    /// </summary>
    public static class ErrorsExtensions
    {
        public static List<Error> ToErrorList(this Error error)
        {
            var errorList = new List<Error>()
            {
                new Error(){ErrorMessage = error.ErrorMessage,ErrorCode = error.ErrorCode,ErrorKey = error.ErrorKey}
            };

            return errorList;
        }
    }
}
