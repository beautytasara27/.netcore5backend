using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace authentication.Helpers
{
    public class PasswordHasher
    {
        public static string HashText(string text, string salt)
        {
            HashAlgorithm sha = SHA256.Create();
            byte[] textWithSaltBytes = Encoding.UTF8.GetBytes(string.Concat(text, salt));
            byte[] hashedBytes = sha.ComputeHash(textWithSaltBytes);
            sha.Clear();
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
