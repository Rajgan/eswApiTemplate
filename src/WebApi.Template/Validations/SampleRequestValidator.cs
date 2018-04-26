using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WebApi.Template.Model.DTO;

namespace WebApi.Template.Validations
{
    public class SampleRequestValidator : AbstractValidator<SampleRequest>
    {
        public SampleRequestValidator()
        {
            ValidateModel();
        }

        private void ValidateModel()
        {
            //Default Validation Rules
            RuleFor(x => x.ClientCode).NotEmpty().WithErrorCode("1001");
            RuleFor(x => x.ClientNo).NotEmpty().WithErrorCode("1001");
            RuleFor(x => x.Prefix).NotEmpty().WithErrorCode("1001");

            RuleSet("ValidateForPost", () =>
            {
                RuleFor(x => x.ClientCode).NotEmpty().WithErrorCode("1001")
                                          .MaximumLength(10).WithErrorCode("1020");
                RuleFor(x => x.ClientNo).NotEmpty().WithErrorCode("1001")
                                        .GreaterThan(10).WithErrorCode("1021")
                                        .WithMessage("Client Code Should be greater than 10");
                RuleFor(x => x.Prefix).NotEmpty().WithErrorCode("1001");
            });

            RuleSet("ValidateForPut", () =>
            {
                RuleFor(x => x.ClientCode).NotEmpty().WithErrorCode("1001")
                                          .MaximumLength(20).WithErrorCode("1020");
                RuleFor(x => x.ClientNo).NotEmpty().WithErrorCode("1001")
                                        .GreaterThan(11).WithErrorCode("1021")
                                        .WithMessage("Client Code Should be greater than 10");
                RuleFor(x => x.Prefix).NotEmpty().WithErrorCode("1001");
            });
        }
    }
}
