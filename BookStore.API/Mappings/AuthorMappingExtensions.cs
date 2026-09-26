namespace BookStore.API.Mappings
{
    public static class AuthorMappingExtensions
    {
        public static AuthorResponse ToResponse(this Author author) => new()
        {
            Id = author.Id,
            Name = author.Name,
            Bio = author.Bio,
            BookCount = author.Books.Count
        };

        public static Author ToEntity(this CreateAuthorRequest request) => new()
        {
            Name = request.Name,
            Bio = request.Bio
        };

        public static void ApplyUpdate(
            this Author author,
            UpdateAuthorRequest request)
        {
            author.Name = request.Name;
            author.Bio = request.Bio;
        }
    }
}
