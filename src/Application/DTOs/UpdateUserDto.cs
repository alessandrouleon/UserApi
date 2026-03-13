namespace UserApication.DTOs;

/// <summary>Inbound payload for updating an existing user. Password is optional.</summary>
public sealed record UpdateUserDto(
    string Name,
    string Email,
    string? Password = null
);
