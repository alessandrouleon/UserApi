using UserApin.Common;
using UserApin.Validators;
using UserApin.ValueObjects;

namespace UserApin.Entities;

/// <summary>
/// User aggregate root. All mutations go through domain methods.
/// Private setters guarantee that the entity is never in an invalid state.
/// </summary>
public sealed class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public Password Password { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    // Required by EF Core
    private User() { }

    /// <summary>Factory method — validates domain rules before constructing the aggregate.</summary>
    public static Result<User> Create(string name, string email, Password password)
    {
        var errors = UserValidator.ValidateForCreation(name, email);
        if (errors.Count > 0)
            return Result<User>.Failure(string.Join(" | ", errors));

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            Password = password,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        return Result<User>.Success(user);
    }

    /// <summary>Updates mutable profile fields. Password update is optional.</summary>
    public Result Update(string name, string email, Password? newPassword = null)
    {
        var errors = UserValidator.ValidateForUpdate(name, email);
        if (errors.Count > 0)
            return Result.Failure(string.Join(" | ", errors));

        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();

        if (newPassword is not null)
            Password = newPassword;

        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    /// <summary>Soft-deletes the user by setting the deletion timestamp.</summary>
    public void Delete()
    {
        IsActive = false;
        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
