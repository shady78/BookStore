namespace BookStore.API.Exceptions
{
    // 409
    public class ConflictException : AppException
    {
        public ConflictException(string message) : base(message) { }
    }
}
