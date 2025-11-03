using FluentValidation;
using ShelterManager.Services.Dtos.DailyTask;

namespace ShelterManager.Services.Validators.DailyTask;

public class UpdateDefaultDailyTaskEntryDtoValidator : AbstractValidator<UpdateDefaultDailyTaskEntryDto>
{
    public UpdateDefaultDailyTaskEntryDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters long")
            .MaximumLength(30).WithMessage("Title must not exceed 30 characters");
        
        RuleFor(x => x.Description)
            .MaximumLength(100).WithMessage("Description must not exceed 100 characters");
    }
}