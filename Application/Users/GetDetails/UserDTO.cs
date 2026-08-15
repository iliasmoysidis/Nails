namespace Application.Users.GetDetails;

public sealed record UserDTO(
    int Id,
    string FullName,
    string Email,
    string Phone
);
