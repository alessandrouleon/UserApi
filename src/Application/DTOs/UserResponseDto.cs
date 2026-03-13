namespace UserApication.DTOs;

/// <summary>Response shape returned to the client. Password is never included.</summary>
public sealed record UserResponseDto(
    Guid Id,
    string Name,
    string Email,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
