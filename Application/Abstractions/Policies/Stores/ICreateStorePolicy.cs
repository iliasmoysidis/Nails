namespace Application.Abstractions.Policies.Stores;

public interface ICreateStorePolicy
{
    void EnsureCanCreate();
}