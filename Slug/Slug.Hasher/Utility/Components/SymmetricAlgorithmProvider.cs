using System;
using System.IO;
using System.Security.Cryptography;
using Slug.Hasher.Utility.Components.Interfaces;

namespace Slug.Hasher.Utility.Components
{
    /// <summary>
    ///     ''' Provides cryptographic functions for any <see cref="SymmetricAlgorithm" />.
    ///     ''' By default, this provider uses the <see cref="RijndaelManaged" /> (AES) algorithm.
    ///     '''
    /// </summary>
    /// '''
    /// <remarks>
    ///     ''' The IV (Initialization Vector) is randomly generated and prepended to the resulting
    ///     ''' ciphertext. This is because the IV ultimately uses the same distribution as the
    ///     ''' ciphertext.
    ///     '''
    /// </remarks>
    public class SymmetricAlgorithmProvider : ISymmetricCryptoProvider
    {
        private readonly SymmetricAlgorithm _algorithm;
        private readonly int _ivSize;

        /// <summary>
        ///     ''' Creates an instance using <see cref="RijndaelManaged" /> as the algorithm.
        ///     '''
        /// </summary>
        /// '''
        /// <param name="key">The key to use for this algorithm.</param>
        public SymmetricAlgorithmProvider(byte[] key) : this(Rijndael.Create(), key)
        {
        }

        /// <summary>
        ///     ''' Creates an instance with a specified algorithm and key.
        ///     '''
        /// </summary>
        /// '''
        /// <param name="algorithm">The algorithm to use for cryptographic functions.</param>
        /// '''
        /// <param name="key">The key to use for this algorithm.</param>
        public SymmetricAlgorithmProvider(SymmetricAlgorithm algorithm, byte[] key)
        {
            _algorithm = algorithm;
            algorithm.Key = key;
            algorithm.GenerateIV();
            _ivSize = algorithm.IV.Length;
        }

        /// <summary>
        ///     ''' Encrypts a plaintext value.
        ///     '''
        /// </summary>
        /// '''
        /// <param name="plaintext">The value to encrypt.</param>
        /// '''
        /// <returns>The resulting ciphertext.</returns>
        public byte[] Encrypt(byte[] plaintext)
        {
            ValidateByteArrayParam("plaintext", plaintext);

            _algorithm.GenerateIV();

            byte[] ciphertext;
            using (var transform1 = _algorithm.CreateEncryptor())
            {
                ciphertext = Transform(transform1, plaintext);
            }

            return PrependIvToCipher(ciphertext);
        }

        /// <summary>
        ///     ''' Decrypts ciphertext.
        ///     '''
        /// </summary>
        /// '''
        /// <param name="ciphertext">The ciphertext to decrypt.</param>
        /// '''
        /// <returns>The resulting plaintext.</returns>
        public byte[] Decrypt(byte[] ciphertext)
        {
            ValidateByteArrayParam("ciphertext", ciphertext);

            _algorithm.IV = GetIvFromCipher(ciphertext);

            byte[] plaintext;
            using (var transform1 = _algorithm.CreateDecryptor())
            {
                plaintext = Transform(transform1, StripIvFromCipher(ciphertext));
            }

            return plaintext;
        }

        private byte[] Transform(ICryptoTransform transformation, byte[] buffer)
        {
            byte[] returnBuffer;

            using (var stream = new MemoryStream())
            {
                CryptoStream cryptoStream = null;
                try
                {
                    cryptoStream = new CryptoStream(stream, transformation, CryptoStreamMode.Write);
                    cryptoStream.Write(buffer, 0, buffer.Length);
                    cryptoStream.FlushFinalBlock();
                    returnBuffer = stream.ToArray();
                }
                finally
                {
                    if (cryptoStream != null)
                        cryptoStream.Close();
                }
            }

            return returnBuffer;
        }

        private void ValidateByteArrayParam(string paramName, byte[] value)
        {
            if (value == null || value.Length == 0)
                throw new ArgumentNullException(paramName);
        }

        private byte[] PrependIvToCipher(byte[] ciphertext)
        {
            var buffer1 = new byte[ciphertext.Length + (_algorithm.IV.Length - 1) + 1];
            Buffer.BlockCopy(_algorithm.IV, 0, buffer1, 0, _algorithm.IV.Length);
            Buffer.BlockCopy(ciphertext, 0, buffer1, _algorithm.IV.Length, ciphertext.Length);

            return buffer1;
        }

        private byte[] GetIvFromCipher(byte[] ciphertext)
        {
            var buffer1 = new byte[_ivSize - 1 + 1];
            Buffer.BlockCopy(ciphertext, 0, buffer1, 0, _ivSize);
            return buffer1;
        }

        private byte[] StripIvFromCipher(byte[] ciphertext)
        {
            var buffer1 = new byte[ciphertext.Length - _ivSize - 1 + 1];
            Buffer.BlockCopy(ciphertext, _ivSize, buffer1, 0, buffer1.Length);
            return buffer1;
        }
    }
}