using UserApin.Common;
using UserApin.Validators;
using UserApin.ValueObjects;

namespace UserApin.Entities;
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

    private User() { }

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

    public void Delete()
    {
        IsActive = false;
        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
