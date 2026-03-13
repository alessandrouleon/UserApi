namespace UserApication.Interfaces;

public interface IHashService
{
    string Hash(string plainText);
    bool Verify(string plainText, string hash);
}
