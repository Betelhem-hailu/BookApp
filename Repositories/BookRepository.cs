using BookStore.Context;
using Microsoft.EntityFrameworkCore;
using BookStore.Repositories.Interfaces;
using BookStore.Contracts;
using AutoMapper;

public class BookRepository : IBookRepository
{
    private readonly BookDbContext _context;
    private readonly ILogger<BookRepository> _logger;
    private readonly IMapper _mapper;

    public BookRepository(BookDbContext context, ILogger<BookRepository> logger, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public Task<Book?> GetBookByISBNAsync(string ISBN, CancellationToken cancellationToken)
            {
                return _context.Books
                            .Include(x => x.Title)
                            .Include(x => x.Author)
                            .FirstOrDefaultAsync(x => x.ISBN == ISBN, cancellationToken);
            }

    public Task<bool> AnyAsync(string ISBN, CancellationToken cancellationToken)
    {
         _logger.LogInformation("Checking if a book with ISBN {ISBN} exists.", ISBN);
        // _logger.Info(ISBN);
        return _context.Books.AnyAsync(x => x.ISBN == ISBN, cancellationToken);
    }


    public async Task<Book> AddNewBook(Book request)
    {
        try
            {
                var book = _mapper.Map<Book>(request);
                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();
                return book;
            }
    
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the book item.");
                throw new Exception("An error occurred while creating the book item.");
            }
    }
}
