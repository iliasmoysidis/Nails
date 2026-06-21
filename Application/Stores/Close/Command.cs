using MediatR;

namespace Application.Stores.Close;

public sealed record Command(int StoreId) : IRequest;