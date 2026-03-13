namespace UserApication.Interfaces;

/// <summary>
/// Application-level interface for password hashing.
/// Keeps the hashing algorithm (bcrypt) isolated in Infrastructure.
/// </summary>
public interface IHashService
{
    string Hash(string plainText);
    bool Verify(string plainText, string hash);
}
