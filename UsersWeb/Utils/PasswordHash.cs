using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using UsersWeb.Utils;

namespace UsersWeb
{
    public static class PasswordHash
    {
        public static string GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using var rngCsp = RandomNumberGenerator.Create();
            rngCsp.GetNonZeroBytes(salt);
            return ByteConverter.GetHexString(salt);
        }

        public static string GetPasswordHash(string salt, string password)
        {
            byte[] saltHash = ByteConverter.GetHexBytes(salt);
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltHash,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100000,
                numBytesRequested: 512 / 8));
        }
    }
}
