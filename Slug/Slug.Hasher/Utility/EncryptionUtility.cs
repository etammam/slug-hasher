using System;
using System.Text;
using Slug.Hasher.Utility.Components;
using Slug.Hasher.Utility.Components.Interfaces;
using Slug.Hasher.Utility.Interfaces;

namespace Slug.Hasher.Utility
{
    /// <summary>
    ///     ''' Provides a general encrypt and decrypt method, requires a 16 char key
    ///     '''
    /// </summary>
    public class EncryptionUtility : IEncryptionUtility
    {
        private readonly byte[] _key;
        private readonly IHashProvider _mHashProvider;
        private readonly ISymmetricCryptoProvider _mSymmetricCryptoProvider;

        /// <summary>
        ///     ''' Creates an instance with a specified key.
        ///     '''
        /// </summary>
        /// '''
        /// <param name="key">The key used for cryptographic functions, required 16 chars in length.</param>
        public EncryptionUtility(string key)
        {
            var keyValue = key.Left(16);
            _key = Encoding.UTF8.GetBytes(keyValue);
            _mSymmetricCryptoProvider = new SymmetricAlgorithmProvider(_key);
            _mHashProvider = new HashAlgorithmProvider();
        }

        public byte[] CombineBytes(byte[] buffer1, byte[] buffer2)
        {
            byte[] combinedBytes;

            try
            {
                combinedBytes = new byte[buffer1.Length + (buffer2.Length - 1) + 1];
                Buffer.BlockCopy(buffer1, 0, combinedBytes, 0, buffer1.Length);
                Buffer.BlockCopy(buffer2, 0, combinedBytes, buffer1.Length, buffer2.Length);
            }
            catch (Exception)
            {
                throw new ApplicationException("combine method failed");
            }

            return combinedBytes;
        }

        public string Decrypt(string input)
        {
            byte[] plainText;

            try
            {
                var inputBytes = Convert.FromBase64String(input);

                var hashLength = inputBytes[0];
                var hash = new byte[hashLength - 1 + 1];
                Buffer.BlockCopy(inputBytes, 1, hash, 0, hashLength);
                var cipherText = new byte[inputBytes.Length - hashLength - 2 + 1];
                Buffer.BlockCopy(inputBytes, hashLength + 1, cipherText, 0, cipherText.Length);
                var compareHash = _mHashProvider.Hash(CombineBytes(_key, cipherText));

                if (!CommonUtil.CompareBytes(hash, compareHash))
                    throw new ApplicationException("input value was improperly signed or tampered with");

                plainText = _mSymmetricCryptoProvider.Decrypt(cipherText);
            }
            catch (Exception)
            {
                throw new ApplicationException("decrypt method failed");
            }

            return Encoding.Unicode.GetString(plainText);
        }

        public string Encrypt(string input)
        {
            byte[] hashLength;
            byte[] signedBytes;

            try
            {
                var cipherText = _mSymmetricCryptoProvider.Encrypt(Encoding.Unicode.GetBytes(input));
                var hash = _mHashProvider.Hash(CombineBytes(_key, cipherText));
                signedBytes = CombineBytes(hash, cipherText);
                hashLength = new[] {Convert.ToByte(hash.Length)};
            }
            catch (Exception)
            {
                throw new ApplicationException("encrypt method failed");
            }

            return Convert.ToBase64String(CombineBytes(hashLength, signedBytes));
        }
    }
}