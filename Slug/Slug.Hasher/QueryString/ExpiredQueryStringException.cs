using System;

namespace Slug.Hasher.QueryString
{
    /// <summary>
    ///     ''' Thrown when a queryString has expired and is therefore no longer valid.
    ///     '''
    /// </summary>
    public class ExpiredQueryStringException : Exception
    {
        private ExpiredQueryStringException()
        {
        }

        public static ExpiredQueryStringException CreateInstance()
        {
            return new ExpiredQueryStringException();
        }
    }
}