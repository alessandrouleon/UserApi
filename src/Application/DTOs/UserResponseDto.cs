namespace UserApication.DTOs;

public sealed record UserResponseDto(
    Guid Id,
    string Name,
    string Email,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
