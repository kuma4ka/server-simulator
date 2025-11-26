using FluentValidation;
using ServerSimulator.Library.Configurations;

namespace ServerSimulator.Library.Validators;

public class ServerConfigurationValidator : AbstractValidator<ServerConfiguration>
{
    public ServerConfigurationValidator()
    {
        RuleFor(x => x.ServerId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.MaxCapacity).GreaterThan(0);
        RuleFor(x => x.CurrentOccupancy).GreaterThanOrEqualTo(0).LessThanOrEqualTo(x => x.MaxCapacity);
        RuleFor(x => x.Ip).NotEmpty().Matches(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$");
        RuleFor(x => x.LoadThreshold)
            .InclusiveBetween(0.1f, 1.0f)
            .WithMessage("LoadThreshold must be between 0.1 (10%) and 1.0 (100%)");
    }
}