namespace BookStore.API.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly BookStoreDbContext _context;
        public AuthorService(BookStoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AuthorResponse>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            // select * from authors 
            // select a.Id , a.Name , a.Bio, B.BookCount count(*) from authors jo
            // projection
            return await _context.Authors
                 .AsNoTracking()
                 .Select(a => new AuthorResponse
                 {
                     Id = a.Id,
                     Name = a.Name,
                     Bio = a.Bio,
                     BookCount = a.Books.Count
                 })
                 .ToListAsync(cancellationToken);
        }

        public async Task<AuthorResponse> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var author = await _context.Authors
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new AuthorResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Bio = a.Bio,
                    BookCount = a.Books.Count
                }).FirstOrDefaultAsync(cancellationToken);

            return author ?? throw new NotFoundException("Author", id);
        }

        public async Task<AuthorResponse> CreateAsync(
            CreateAuthorRequest request,
            CancellationToken cancellationToken = default)
        {
            var author = request.ToEntity();
            _context.Authors.Add(author);
            await _context.SaveChangesAsync(cancellationToken);
            return author.ToResponse();
        }

        public async Task DeleteAsync(int id,
            CancellationToken cancellationToken = default)
        {
            var author =await _context.Authors.FirstOrDefaultAsync(a => a.Id == id, cancellationToken)
                ?? throw new NotFoundException(nameof(Author), id);

            var hasBooks = await _context.Books.AnyAsync(b => b.AuthorId == id,cancellationToken);
            if (hasBooks)
            {
                throw new ConflictException(
                    $"Cannot delete author with id {id} because they have associated books.");
            }
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(int id,
            UpdateAuthorRequest request, 
            CancellationToken cancellationToken = default)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id, cancellationToken)
               ?? throw new NotFoundException(nameof(Author), id);
            author.ApplyUpdate(request);

            //_context.Authors.Update(author);
            await _context.SaveChangesAsync( cancellationToken);

        }
    }
}
