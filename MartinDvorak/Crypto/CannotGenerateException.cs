using System;

namespace Crypto
{
    class CannotGenerateException : InvalidOperationException
    {
        public CannotGenerateException() { }
        public CannotGenerateException(string message): base(message) { }
        public CannotGenerateException(string message, Exception innerException) : base(message, innerException) { }
    }
}
