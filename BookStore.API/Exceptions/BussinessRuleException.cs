namespace BookStore.API.Exceptions
{
    public class BussinessRuleException :AppException
    {
        public BussinessRuleException(string message):base(message)
        {
        }
    }
}
