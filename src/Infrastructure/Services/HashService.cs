using UserApication.Interfaces;

namespace UserApistructure.Services;
public sealed class HashService : IHashService
{
    private const int WorkFactor = 12;

    public string Hash(string plainText)
        => BCrypt.Net.BCrypt.HashPassword(plainText, WorkFactor);

    public bool Verify(string plainText, string hash)
        => BCrypt.Net.BCrypt.Verify(plainText, hash);
}
