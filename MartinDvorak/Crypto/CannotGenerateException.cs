using System;

namespace Crypto
{
    /// <summary>
    /// Custom exception that any key extender can throw.
    /// </summary>
    class CannotGenerateException : InvalidOperationException
    {
        public CannotGenerateException() { }
        public CannotGenerateException(string message): base(message) { }
        public CannotGenerateException(string message, Exception innerException) : base(message, innerException) { }
    }
}
