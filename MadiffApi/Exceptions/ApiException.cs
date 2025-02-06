namespace MadiffApi.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }

        public ApiException(string message, int statusCode = 500) : base(message)
        {
            StatusCode = statusCode;
        }
    }

    public class NotFoundException : ApiException
    {
        public NotFoundException(string message) : base(message, 404)
        {
        }
    }
    public class ValidationException : ApiException
    {
        public ValidationException(string message) : base(message, 400)
        {
        }
    }
}
