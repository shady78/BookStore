namespace BookStore.API.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string resourceName, object key)
            : base($"{resourceName} with id '{key}' was not found.")
        {
        }
    }
}
