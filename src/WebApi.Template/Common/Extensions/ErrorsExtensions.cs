using System.Collections.Generic;
using WebApi.Template.Model.Exceptions;

namespace Labelling.Common.Extensions
{
    /// <summary>
    /// Extension Method to add Errors
    /// </summary>
    public static class ErrorsExtensions
    {
        public static T AddErrors<T>(this T exception, Error error) where T : BaseException
        {
            exception.Errors.Add(error);
            return exception;
        }

        public static T AddErrorList<T>(this T exception, IList<Error> errorList) where T : BaseException
        {
            exception.Errors.AddRange(errorList);
            return exception;
        }
        
    }
}
