namespace Encrption;

public interface ICipherService
{
    string Encrypt(string chipterText);
    string Decrypt(string chipterText);
}