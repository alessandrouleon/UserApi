using UserApin.Entities;

namespace UserApin.Interfaces.Repositories;

/// <summary>
/// Domain contract for User persistence.
/// Implementations live in Infrastructure — the Domain never references EF Core.
/// </summary>
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <returns>Tuple of the current page items and the total record count for pagination.</returns>
    Task<(IEnumerable<User> Items, int TotalCount)> GetAllAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken cancellationToken = default);

    Task AddAsync(User user, CancellationToken cancellationToken = default);
    void Update(User user);
    void Remove(User user);

    /// <param name="excludeId">Optional ID to exclude from the check (used during updates).</param>
    Task<bool> EmailExistsAsync(string email, Guid? excludeId = null, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
