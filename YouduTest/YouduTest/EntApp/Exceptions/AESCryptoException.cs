

namespace YouduTest.EntApp.Exceptions
{
    public class AESCryptoException : GeneralEntAppException
    {
        public AESCryptoException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
