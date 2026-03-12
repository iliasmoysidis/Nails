namespace Application.DTO.User;

public sealed record UserProfileDTO(
    int Id,
    string FullName,
    string Email,
    string Phone
);