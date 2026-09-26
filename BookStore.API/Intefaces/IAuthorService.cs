namespace BookStore.API.Intefaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorResponse>> GetAllAsync(
            CancellationToken cancellationToken = default);
        Task<AuthorResponse> GetByIdAsync(
            int id,CancellationToken cancellationToken = default);
        Task<AuthorResponse> CreateAsync(
            CreateAuthorRequest request,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(int id,
            UpdateAuthorRequest request,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, 
            CancellationToken cancellationToken = default);
    }
}
