using BookStore.API.Services;

namespace BookStore.API.Intefaces
{
    public interface IBookService
    {
        IEnumerable<BookResponse> GetAll();
        ServiceResult<BookResponse> GetById(int id);
        ServiceResult<BookResponse> Create(CreateBookRequest request);
        ServiceResult<bool> Update(int id, UpdateBookRequest request);
        ServiceResult<bool> Delete(int id);
    }
}
