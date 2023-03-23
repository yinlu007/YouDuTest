

namespace YouduTest.EntApp.Exceptions
{
    public class SignatureException : GeneralEntAppException
    {
        public SignatureException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
