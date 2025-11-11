using System.Security.Cryptography;
using System.Text;

namespace SWBackend.ServiceLayer.Auth.StaticMethods;

public static class TokenMethod
{
    /// <summary>
    /// Converte la string in byte, calcola SHA256, ritorna in hash esadecimale
    /// SHA256: criptaggio per evitare di lasciare il token in chiaro, in caso di compromissione del db.
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    public static string GenerateRefreshToken(int size = 64)
    {
        var randomNumber = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    /// <summary>
    /// Converte la stringa in UTF8, serve per salvare l'hash nel db del token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public static string HashToken(string token)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(token);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }
}