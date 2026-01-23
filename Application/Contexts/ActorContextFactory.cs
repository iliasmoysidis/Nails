using Application.Abstractions;
using Application.Contexts;
using Application.Repositories;

public sealed class ActorContextFactory
{
    private readonly IStaffRepository _repo;
    private readonly ICurrentUser _currentUser;

    public ActorContextFactory(IStaffRepository repo, ICurrentUser currentUser)
    {
        _repo = repo;
        _currentUser = currentUser;
    }

    public async Task<ActorContext> CreateAsync(int storeId, CancellationToken ct)
    {
        var staff = await _repo.GetStaffAsync(storeId, ct);

        return new ActorContext(
            _currentUser.UserId,
            staff.IsOwner(_currentUser.UserId),
            staff.IsEmployee(_currentUser.UserId)
        );
    }
}