using FluentValidation;
using ShelterManager.Common.Dtos;

namespace ShelterManager.Services.Validators;

public class PageQueryFilterValidator : AbstractValidator<PageQueryFilter>
{
    private readonly int[] _allowedPageSizes = [10, 25, 50, 100];
    
    public PageQueryFilterValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .Custom((value, context) =>
            {
                if (!_allowedPageSizes.Contains(value))
                {
                    context.AddFailure($"Page Size is invalid. Page size must be in {string.Join(",", _allowedPageSizes)}");
                }
            });
    }
}