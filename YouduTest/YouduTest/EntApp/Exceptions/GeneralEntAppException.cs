

namespace YouduTest.EntApp.Exceptions
{
    public class GeneralEntAppException : System.Exception
    {
        public GeneralEntAppException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
