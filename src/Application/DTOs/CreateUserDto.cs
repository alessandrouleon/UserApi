namespace UserApication.DTOs;

public sealed record CreateUserDto(
    string Name,
    string Email,
    string Password
);
