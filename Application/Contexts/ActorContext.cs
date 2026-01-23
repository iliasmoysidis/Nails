namespace Application.Contexts;

public sealed record ActorContext(int UserId, bool IsOwner, bool IsProfessional);