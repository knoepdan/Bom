using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Ch.Knomes.Security
{
    public class PasswordHashResult
    {
        public PasswordHashResult(byte[] hash, byte[] salt)
        {
            this.Hash = hash;
            this.Salt = salt;
        }

        public byte[] Hash { get; } = new byte[0];

        public byte[] Salt { get; } = new byte[0];

        /// <summary>
        /// Salt as Base64 encoded string
        /// </summary>
        public string HashString => Convert.ToBase64String(this.Hash);

        /// <summary>
        /// Salt as Base64 encoded string
        /// </summary>
        public string SaltString => Convert.ToBase64String(this.Salt);

    }

    public class PasswordHelper
    {
        // private static readonly IReadOnlyList<byte> Pepper = new List<byte> { (byte)2, (byte)0, (byte)5, (byte)8 }; // do not change default pepper, as it would kill version upgrades

        IReadOnlyList<byte>? Pepper { get; set; }

        public PasswordHelper(IReadOnlyList<byte>? pepper = null)
        {
            this.Pepper = pepper;
        }


        public bool CheckPasswordHash(string password, string salt, string expectedHashValue)
        {
            if(expectedHashValue == null)
            {
                throw new ArgumentNullException(nameof(expectedHashValue));
            }

            var result = HashPassword(password, salt);
            if(result.HashString == expectedHashValue)
            {
                return true;
            }
            return false;
        }

        public PasswordHashResult HashPasswordWithRandomSalt(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            byte[] salt = CryptoUtility.GetRandomBytes(128 / 8);
            var result = HashPassword(password, salt);
            return result;
        }

        public PasswordHashResult HashPassword(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var result = HashPassword(password, saltBytes);
            return result;
        }

        public PasswordHashResult HashPassword(string password, byte[] salt)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            var saltAndPepper = salt;
            if (this.Pepper != null)
            {
                var adjustableList = new List<byte>(salt);
                adjustableList.AddRange(this.Pepper);
                saltAndPepper = adjustableList.ToArray();
            }

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            var hashBytes = KeyDerivation.Pbkdf2(
                password: password,
                salt: saltAndPepper,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);
            var result = new PasswordHashResult(hashBytes, salt);
            return result;
        }

    }
}