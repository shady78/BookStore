using BookStore.API.Services;

namespace BookStore.API.Intefaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookResponse>> GetAllAsync(CancellationToken cancellation = default);
        Task<ServiceResult<BookResponse>> GetByIdAsync(int id, CancellationToken cancellation = default);
        Task<ServiceResult<BookResponse>> CreateAsync(CreateBookRequest request, CancellationToken cancellation = default);
        Task<ServiceResult<bool>> UpdateAsync(int id, UpdateBookRequest request, CancellationToken cancellation = default);
        Task<ServiceResult<bool>> DeleteAsync(int id, CancellationToken cancellation = default);
        Task<PagedResult<BookResponse>> GetAllWithQueryAsync(
            BookQueryParamters parameters,
            CancellationToken cancellation = default);
    }
}
