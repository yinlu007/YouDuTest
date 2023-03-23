

namespace YouduTest.EntApp.Exceptions
{
    public class ServiceException : GeneralEntAppException
    {
        public ServiceException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
