using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTier.Shared.Helpers
{
    public class HashedPasswordConfig
    {
        public string HashPassword(string password)
        {
            // Generate a salt and hash the password with it
            string salt = BCrypt.Net.BCrypt.GenerateSalt(12); // You can adjust the salt complexity as needed
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            return hashedPassword;
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Verify a password against its hashed counterpart
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
