using Course.Web.Models.Discounts;
using FluentValidation;

namespace Course.Web.Validator
{
    public class DiscountApplyInputValidator:AbstractValidator<DiscountApplyInput>
    {
        public DiscountApplyInputValidator()
        {
            RuleFor(x => x.Code).NotNull().WithMessage("İndirim Kupun alanı boş olamaz");
        }
    }
}
