using MediatR;

namespace Application.Stores.Close;

public sealed record CloseStoreCommand(int StoreId) : IRequest;