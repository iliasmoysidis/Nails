using MediatR;

namespace Application.Features.Stores.Close;

public sealed record Command(int StoreId) : IRequest;