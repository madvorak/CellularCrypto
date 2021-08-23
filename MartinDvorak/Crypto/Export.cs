using Cellular;

namespace Crypto
{
    /// <summary>
    /// Public static class that gives access to our cryptographical tools.
    /// This class is made to be used from any assembly.
    /// </summary>
    public static class Export
    {
        private static IKeyExtender favouriteExtender;

        static Export()
        {
            favouriteExtender = new KeyExtenderInterlaced(new ElementaryAutomatonFaster(30, 1), 10, 0);
        }

        /// <summary>
        /// Exports a key extender for use by any external program.
        /// </summary>
        /// <returns>Key extender.</returns>
        public static IKeyExtender GetKeyExtender()
        {
            return favouriteExtender;
        }

        /// <summary>
        /// Exports a ready-to-use encrypter/decrypter for use by any external program.
        /// This uses CA-based key extender inside.
        /// </summary>
        /// <returns>Encrypter and decrypter.</returns>
        public static EncryptionProvider GetEncrypterStreamCA()
        {
            return new EncryptionProvider(new EncrypterStreamCA(favouriteExtender));
        }

        /// <summary>
        /// Exports a ready-to-use encrypter/decrypter for use by any external program.
        /// This uses a reversible CA inside.
        /// </summary>
        /// <returns>Encrypter and decrypter.</returns>
        public static EncryptionProvider GetEncrypterReversibleCA()
        {
            return new EncryptionProvider(new EncrypterReversibleCA(1000));
        }
    }
}
