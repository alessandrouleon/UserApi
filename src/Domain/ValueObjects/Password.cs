namespace UserApin.ValueObjects;

/// <summary>
/// Immutable Value Object that wraps a bcrypt password hash.
/// Creation from plain text requires injection of a hash function,
/// keeping the Domain free of infrastructure dependencies.
/// </summary>
public sealed class Password
{
    public string Hash { get; }

    private Password(string hash)
    {
        Hash = hash;
    }

    /// <summary>Creates a Password from an already-hashed string (used when loading from DB).</summary>
    public static Password FromHash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("Password hash cannot be empty.", nameof(hash));

        return new Password(hash);
    }

    /// <summary>Creates a Password by hashing plain text with the provided hash function.</summary>
    public static Password Create(string plainText, Func<string, string> hashFunction)
    {
        if (string.IsNullOrWhiteSpace(plainText))
            throw new ArgumentException("Plain-text password cannot be empty.", nameof(plainText));

        var hash = hashFunction(plainText);
        return new Password(hash);
    }

    /// <summary>Verifies a plain-text candidate against this hash.</summary>
    public bool Verify(string plainText, Func<string, string, bool> verifyFunction)
        => verifyFunction(plainText, Hash);

    public override string ToString() => Hash;

    public override bool Equals(object? obj)
        => obj is Password other && Hash == other.Hash;

    public override int GetHashCode() => Hash.GetHashCode();
}
