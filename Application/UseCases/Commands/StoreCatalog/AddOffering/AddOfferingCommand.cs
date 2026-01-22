using Application.Abstractions;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Offerings;
using Domain.ValueObjects.Time;

namespace Application.UseCases.Commands.StoreCatalog.AddOffering;

public sealed record AddOfferingCommand(int StoreId, OfferingName Name, Money Price, Duration Duration, string? Description) : ICommand;