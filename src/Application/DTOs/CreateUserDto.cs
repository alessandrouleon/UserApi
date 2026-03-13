namespace UserApication.DTOs;

/// <summary>Inbound payload for creating a new user (Presentation → Application).</summary>
public sealed record CreateUserDto(
    string Name,
    string Email,
    string Password
);
