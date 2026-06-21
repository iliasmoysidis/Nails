namespace Application.Users.GetProfile;

public sealed record UserProfileDTO(
    int Id,
    string FullName,
    string Email,
    string Phone
);