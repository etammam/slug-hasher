using System.Security.Cryptography;
using Slug.Hasher.Utility.Components.Interfaces;

namespace Slug.Hasher.Utility.Components
{
    /// <summary>
    ///     ''' Provides hash functions for any <see cref="HashAlgorithm" />. By default,
    ///     ''' this provider uses the <see cref="MD5" /> algorithm.
    ///     '''
    /// </summary>
    public class HashAlgorithmProvider : IHashProvider
    {
        private readonly HashAlgorithm _algorithm;

        /// <summary>
        ///     ''' Creates a default instance of this provider.
        ///     '''
        /// </summary>
        public HashAlgorithmProvider() : this(MD5.Create())
        {
        }

        /// <summary>
        ///     ''' Creates an instance with a specified hash algorithm.
        ///     '''
        /// </summary>
        /// '''
        /// <param name="algorithm">The algorithm to use to compute the hash.</param>
        public HashAlgorithmProvider(HashAlgorithm algorithm)
        {
            _algorithm = algorithm;
        }

        /// <summary>
        ///     ''' Computes the hash value of a byte array using the specified algorithm.
        ///     '''
        /// </summary>
        /// '''
        /// <param name="buffer">The buffer to hash.</param>
        /// '''
        /// <returns>The computed hash.</returns>
        public byte[] Hash(byte[] buffer)
        {
            return _algorithm.ComputeHash(buffer);
        }
    }
}