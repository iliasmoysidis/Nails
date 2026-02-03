namespace Application.Abstractions.Policies.Staffs;

public interface IManageStaffPolicy
{
    Task EnsureCanManageStaffAsync(int storeId, CancellationToken ct);
}