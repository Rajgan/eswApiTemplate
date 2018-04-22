using System.Linq;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Labelling.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using WebApi.Template.Model.Exceptions;
using ValidationException = WebApi.Template.Model.Exceptions.ValidationException;

namespace WebApi.Template.Validations
{
    /// <summary>
    /// Base Validator and Interceptor to handle Validation Error
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public class BaseValidator<TRequest> : AbstractValidator<TRequest>, IValidatorInterceptor
    {
        public BaseValidator()
        {
            // Default the Cascade mode for all validation on the API.
            CascadeMode = CascadeMode.StopOnFirstFailure;
        }
        public ValidationContext BeforeMvcValidation(ControllerContext controllerContext, ValidationContext validationContext)
        {
            return validationContext;
        }

        public ValidationResult AfterMvcValidation(ControllerContext controllerContext, ValidationContext validationContext, ValidationResult result)
        {
            if (!result.IsValid)
            {
                var errorList = result.Errors.Select(x => new Error()
                {
                    ErrorKey = x.PropertyName,
                    ErrorMessage = x.ErrorMessage,
                    ErrorCode = x.ErrorCode
                }).ToList();

                throw new ValidationException().AddErrorList(errorList);
                
            }

            return result;
        }
    }
}
