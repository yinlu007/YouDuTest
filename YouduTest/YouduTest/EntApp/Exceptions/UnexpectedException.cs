

namespace YouduTest.EntApp.Exceptions
{
    public class UnexpectedException : GeneralEntAppException
    {
        public UnexpectedException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
