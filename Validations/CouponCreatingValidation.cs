using FluentValidation;
using MinimalAPIs.Models;

namespace MinimalAPIs.Validations
{
    public class CouponCreatingValidation : AbstractValidator<CouponCreateDTO>
    {
        public CouponCreatingValidation()
        {

            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Percent).InclusiveBetween(1, 100);

        }
    }
}
