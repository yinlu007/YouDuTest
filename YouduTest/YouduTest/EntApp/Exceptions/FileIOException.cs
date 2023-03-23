

namespace YouduTest.EntApp.Exceptions
{
    public class FileIOException : GeneralEntAppException
    {
        public FileIOException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
