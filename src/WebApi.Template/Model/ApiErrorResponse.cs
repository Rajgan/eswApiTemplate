using System.Collections.Generic;
using WebApi.Template.Model.Common;
using WebApi.Template.Model.Exceptions;

namespace WebApi.Template.Model
{
    public class ApiErrorResponse 
    {
        /// <summary>
        /// Determine what type of Error Business error OR Validation Error OR InternalServer Error
        /// </summary>
        public ErrorTypeCode ErrorType { get; set; }
        /// <summary>
        /// Hold Error information
        /// </summary>
        public List<Error> ErrorDetails { get; set; }

        /// <summary>
        /// Populate ErrorIdentifier with Application Insight Opertion Id
        /// </summary>
        public string ErrorIdentifier { get; set; }
        
    }
}
