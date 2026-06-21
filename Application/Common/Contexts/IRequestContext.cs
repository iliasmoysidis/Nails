namespace Application.Common.Contexts;

public interface IRequestContext
{
    int ActorId { get; }

    bool IsUser { get; }
    bool IsProfessional { get; }
}