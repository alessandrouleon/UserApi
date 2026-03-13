namespace UserApication.DTOs;

public sealed record UpdateUserDto(
    string Name,
    string Email,
    string? Password = null
);
