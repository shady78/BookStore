using BookStore.API.Data;
using BookStore.API.Mappings;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStore.API.Services
{
    public class BookService : IBookService
    {
        private readonly BookStoreDbContext _context;

        public BookService(BookStoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookResponse>> GetAllAsync(CancellationToken cancellation = default)
        {
            //// query build 
            //IQueryable<Book> query = _context.Books.Where(b => b.Price > 1000);
            //query = query.Where(b => b.StockQuantity > 0);
            //// select , orderyby , where 
            //var result = await query.ToListAsync();
            // TolistAsync() , FirstOrDefaultAsync() ,CountAsync()

            // IEnumberable 
            //IEnumerable<Book> books = await _context.Books.ToListAsync(); // 10000
            // var expensiveBooks = books.Where(b => b.Price > 1000); // 10



            // Modified , Added , Deleted  , NoChange
            var books = await _context.Books.AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.BookCategories)
                    .ThenInclude(bc => bc.Category)
                .ToListAsync();
            return books.ToResponseList();
        }

        public async Task<ServiceResult<BookResponse>> GetByIdAsync(int id, CancellationToken cancellation)
        {
            var book = await _context.Books.AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.BookCategories)
                    .ThenInclude(bc => bc.Category)
                .FirstOrDefaultAsync(b => b.Id == id);

            return book is null ?
                ServiceResult<BookResponse>.Fail(errorMessage: "Book not found") :
                ServiceResult<BookResponse>.Ok(book.ToRespose());
        }
        public async Task<ServiceResult<BookResponse>> CreateAsync(CreateBookRequest request, CancellationToken cancellation)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return ServiceResult<BookResponse>.Fail(errorMessage: "Title is required.");
            }
            if (request.Price <= 0)
            {
                return ServiceResult<BookResponse>.Fail(errorMessage: "Price must be greater than zero.");
            }
            var book = request.ToEntity();
            _context.Books.Add(book);
            await _context.SaveChangesAsync(cancellation);
            return ServiceResult<BookResponse>.Ok(book.ToRespose());
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id, CancellationToken cancellation)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book is null)
                ServiceResult<BookResponse>.Fail(errorMessage: "Book not found");

            _context.Books.Remove(book!);
            await _context.SaveChangesAsync(cancellation);
            return ServiceResult<bool>.Ok(true);
        }


        public async Task<ServiceResult<bool>> UpdateAsync(int id, UpdateBookRequest request, CancellationToken cancellation)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book is null)
                return ServiceResult<bool>.Fail(errorMessage: "Book not found");

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return ServiceResult<bool>.Fail(errorMessage: "Title is required.");
            }
            if (request.Price <= 0)
            {
                return ServiceResult<bool>.Fail(errorMessage: "Price must be greater than zero.");
            }
            book.ApplyUpdate(request);
            await _context.SaveChangesAsync(cancellation);
            return ServiceResult<bool>.Ok(true);
        }

        public async Task<PagedResult<BookResponse>> GetAllWithQueryAsync(
            BookQueryParamters parameters,
            CancellationToken cancellation = default)
        {
            var query = _context.Books.AsNoTracking().
                 Include(b => b.Author)
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim();
                query = query.Where(b => b.Title.Contains(search));
            }
            if (parameters.AuthorId.HasValue)
            {
                query = query.Where(b => b.AuthorId
                == parameters.AuthorId.Value);
            }
            if (parameters.CategoryId.HasValue)
            {
                query = query.Where(b => b.BookCategories.Any(
                    bc => bc.CategoryId == parameters.CategoryId.Value));
            }
            if (parameters.MinPrice.HasValue)
            {
                query = query.Where(b => b.Price >= parameters.MinPrice.Value);
            }
            if (parameters.MaxPrice.HasValue)
            {
                query = query.Where(b => b.Price <= parameters.MaxPrice.Value);
            }
            var totalCount = await query.CountAsync(cancellation);

            query = ApplySorting(query, parameters.SortBy, parameters.Descending);
            // pageNumber = 4 , pageSize = 10  
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .Select(b => b.ToRespose())
                .ToListAsync();

            return new PagedResult<BookResponse>(
                items, parameters.PageNumber, parameters.PageSize, totalCount);
        }
        // helper method
        private static IQueryable<Book> ApplySorting(
            IQueryable<Book> query, string? sortBy, bool descending)
        {
            Expression<Func<Book, object>> keySelector = sortBy?.ToLower() switch
            {
                "price" => b => b.Price,
                "stock" => b => b.StockQuantity,
                _ => b => b.Title
            };
            return descending ? query.OrderByDescending(keySelector) :
                query.OrderBy(keySelector);
        }
    }
}


// AsNoTracking 