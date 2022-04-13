using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.DataProtection;

namespace Encrption;

public class Security :ICipherService
{
    private readonly IDataProtectionProvider _dataProtectionProvider;
    private const string Key = "CutTheNightWithTheLight";                                 

    public Security(IDataProtectionProvider dataProtectionProvider)
    {
        _dataProtectionProvider = dataProtectionProvider;
    }
    
    public string Encrypt(string input)
    {
        if (string.IsNullOrEmpty(input) || input == "null")
            return string.Empty;

        using (var provider = new TripleDESCryptoServiceProvider())
        {
            provider.Key = Encoding.ASCII.GetBytes(Key.Substring(0, 16));
            provider.IV = Encoding.ASCII.GetBytes(Key.Substring(8, 8));

            var encryptedBinary = EncryptTextMemory(input, provider.Key, provider.IV);
            return Convert.ToBase64String(encryptedBinary);
        }
    }

    private byte[] EncryptTextMemory(string data, byte[] key, byte[] iv)
    {
        using (var ms = new MemoryStream())
        {
            using (var cs = new CryptoStream(ms,new TripleDESCryptoServiceProvider().CreateEncryptor(key,iv),CryptoStreamMode.Write))
            {
                var toEncrypt = Encoding.Unicode.GetBytes(data);
                cs.Write(toEncrypt,0,toEncrypt.Length);
                cs.FlushFinalBlock();
            }

            return ms.ToArray();
        }
    }

    public string Decrypt(string input)
    {
        try
        {
            if (string.IsNullOrEmpty(input) || input == "null")
                return string.Empty;

            using (var provider = new TripleDESCryptoServiceProvider())
            {
                provider.Key = Encoding.ASCII.GetBytes(Key.Substring(0, 16));
                provider.IV = Encoding.ASCII.GetBytes(Key.Substring(8, 8));

                var buffer = Convert.FromBase64String(input);
                return DecryptTextMemory(buffer, provider.Key, provider.IV);
            }
        }
        catch (Exception e)
        {
            return input;
        }
    }

    private string DecryptTextMemory(byte[] data, byte[] key, byte[] iv)
    {
        using (var ms = new MemoryStream(data))
        {
            using (var cs = new CryptoStream(ms,new TripleDESCryptoServiceProvider().CreateDecryptor(key,iv),CryptoStreamMode.Read))
            {
                using (var sr = new StreamReader(cs,Encoding.Unicode))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}