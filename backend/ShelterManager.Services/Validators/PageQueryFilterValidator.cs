using FluentValidation;
using ShelterManager.Services.Dtos.Commons;

namespace ShelterManager.Services.Validators;

public class PageQueryFilterValidator : AbstractValidator<PageQueryFilter>
{
    // TODO: 10000000 to handle fetching all items from list
    private readonly int[] _allowedPageSizes = [5, 10, 25, 50, 100, 10000000];
    
    public PageQueryFilterValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Page must be greater than or equal to 1.");

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