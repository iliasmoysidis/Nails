namespace Application.Contexts;

public interface IRequestContext
{
    int ActorId { get; }

    bool IsUser { get; }
    bool IsProfessional { get; }
}