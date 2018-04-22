using FluentValidation;
using WebApi.Template.Model.Domain;

namespace WebApi.Template.Validations
{
    public class NameAddressesValidator : BaseValidator<NameAddresses>
    {
        public NameAddressesValidator()
        {
            ValidateModel();
        }

        private void ValidateModel()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithErrorCode("1001");
            RuleFor(x => x.LastName).NotEmpty().WithErrorCode("1001");
            RuleFor(x => x.Email).NotEmpty().WithErrorCode("1001")
                                 .EmailAddress().WithErrorCode("1002");
            RuleFor(x => x.Address1).NotEmpty().WithErrorCode("1001");
            RuleFor(x => x.City).NotEmpty().WithErrorCode("1001");
           
        }
    }
}
