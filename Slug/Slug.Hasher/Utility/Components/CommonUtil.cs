namespace Slug.Hasher.Utility.Components
{
    /// <summary>
    ///     ''' Utility class for common functions.
    ///     '''
    /// </summary>
    public static class CommonUtil
    {
        /// <summary>
        ///     ''' Compares a set of byte arrays to determine if they are equal or not.
        ///     '''
        /// </summary>
        /// '''
        /// <param name="array1">The first array to be compared.</param>
        /// '''
        /// <param name="array2">The second array to be compared.</param>
        /// '''
        /// <returns><c>true</c> if the byte arrays are of equal value. Otherwise <c>false</c>.</returns>
        public static bool CompareBytes(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
                return false;

            for (var i = 0; i <= array1.Length - 1; i++)
                if (array1[i] != array2[i])
                    return false;
            return true;
        }
    }
}