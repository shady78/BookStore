namespace BookStore.API.DTOs
{
    public class AuthorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public int BookCount { get; set; }
    }

    public class CreateAuthorRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
    }
    public class UpdateAuthorRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
    }

}
