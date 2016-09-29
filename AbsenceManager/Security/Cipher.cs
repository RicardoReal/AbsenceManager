using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace AbsenceManager.Security
{
    public class Cipher
    {
        /// <summary>
        /// Compares a hash of the specified plain text value to a given hash value.
        /// </summary>
        /// <param name="PlainText">Plain text to be verified against the specified hash. The function does not check whether this parameter is null.</param>
        /// <param name="HashAlgorithm">Name of the hash algorithm: MD5, SHA, SHA1, SHA256, SHA384, SHA512. This value is case-insensitive. Defaults to SHA.</param>
        /// <param name="HashValue">Base64-encoded hash value produced by ComputeHash function.</param>
        /// <param name="Valid">If computed hash matches the specified hash the function the return value is true; otherwise, the function returns false.</param>
        public void HashVerify(string PlainText, string HashAlgorithm, string HashValue, out bool Valid)
        {
            Valid = false;

            string hasValue;
            HashCompute(PlainText, HashAlgorithm, out hasValue);

            Valid = (hasValue == HashValue);
        }

        /// <summary>
        ///  Generates a hash for the given plain text value and returns a base64-encoded result.
        /// </summary>
        /// <param name="PlainText">Plaintext value to be hashed. The function does not check whether this parameter is null.</param>
        /// <param name="_HashAlgorithm">Name of the hash algorithm: MD5, SHA, SHA1, SHA256, SHA384, SHA512. This value is case-insensitive. Defaults to SHA.</param>
        /// <param name="HashValue">Hash value formatted as a base64-encoded string.</param>
        public void HashCompute(string PlainText, string _HashAlgorithm, out string HashValue)
        {
            HashValue = String.Empty;

            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(PlainText);

            // Because we support multiple hashing algorithms, we must define
            // hash object as a common (abstract) base class. We will specify the
            // actual hashing algorithm class later during object creation.
            HashAlgorithm hash = HashAlgorithm.Create(_HashAlgorithm.ToUpper());

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextBytes);

            // Convert result into a base64-encoded string.
            HashValue = Convert.ToBase64String(hashBytes);

        }


        /// <summary>
        /// Decodes a character form compatible format.
        /// </summary>
        /// <param name="inChar">The character to decode</param>
        /// <param name="outChar">The decoded character</param>
        public void ASCIIDecode(int inChar, out string outChar)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            outChar = ascii.GetString(new byte[] { (byte)inChar });
        }


        /// <summary>
        /// Encodes a character in ASCII
        /// </summary>
        /// <param name="inChar">The character to encode</param>
        /// <param name="outChar">The encoded character</param>
        public void ASCIIEncode(string inChar, out int outChar)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            outChar = (int)ascii.GetBytes(inChar)[0];
        }
    }
}