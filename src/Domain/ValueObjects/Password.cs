namespace UserApin.ValueObjects;

public sealed class Password
{
    public string Hash { get; }

    private Password(string hash)
    {
        Hash = hash;
    }

    public static Password FromHash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("Password hash cannot be empty.", nameof(hash));

        return new Password(hash);
    }

    public static Password Create(string plainText, Func<string, string> hashFunction)
    {
        if (string.IsNullOrWhiteSpace(plainText))
            throw new ArgumentException("Plain-text password cannot be empty.", nameof(plainText));

        var hash = hashFunction(plainText);
        return new Password(hash);
    }

    public bool Verify(string plainText, Func<string, string, bool> verifyFunction)
        => verifyFunction(plainText, Hash);

    public override string ToString() => Hash;

    public override bool Equals(object? obj)
        => obj is Password other && Hash == other.Hash;

    public override int GetHashCode() => Hash.GetHashCode();
}
