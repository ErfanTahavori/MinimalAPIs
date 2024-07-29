using FluentValidation;
using MinimalAPIs.Models;

namespace MinimalAPIs.Validations
{
    public class CouponUpdatingValidation : AbstractValidator<CouponUpdateDTO>
    {

        public CouponUpdatingValidation()
        {
            RuleFor(model => model.Id).NotEmpty().GreaterThan(0);
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Percent).InclusiveBetween(1, 100);

        }
    }
}
