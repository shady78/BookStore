namespace BookStore.API.Exceptions
{
    public class RequestValidationException :AppException
    {
        public IDictionary<string, string[]>? Errors { get; }
        public RequestValidationException(
            IDictionary<string, string[]> errors):
            base("One or more validation errors occured.")
        {
            Errors = errors;
        }
    }
}
