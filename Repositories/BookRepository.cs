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

    //Get list of books from database
    public async Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Books
        .Include(b => b.Categories)
        .ToListAsync(cancellationToken);
    }

    //Get list of Books by category
    public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(List<string> categories, CancellationToken cancellationToken)
    {
         // If no categories are passed, return all books
    if (categories == null || !categories.Any())
    {
        return await _context.Books
        .Include(b => b.Categories)
        .ToListAsync(cancellationToken);
    }

    categories = categories.Select(c => c.Trim().ToLower()).ToList();
    // Filter books by the provided list of categories
    var books = await _context.Books
                     .Include(b => b.Categories)
        .Where(b => b.Categories.Any(c => categories.Contains(c.Name)))
        .ToListAsync(cancellationToken);
    return books;
    }

    //Get Book by Id
    public async Task<Book> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Books
                        .Include(b => b.Categories)
                        .FirstOrDefaultAsync(b => b.BookId == id, cancellationToken);
    }

    //Update Book
    public async Task UpdateAsync(Book book, CancellationToken cancellationToken)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync(cancellationToken);
    }

    //delete book
    public async Task DeleteAsync(Book book, CancellationToken cancellationToken)
    {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync(cancellationToken);   
    }

    public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm, CancellationToken cancellationToken)
{
    if (string.IsNullOrWhiteSpace(searchTerm))
    {
        return Enumerable.Empty<Book>();
    }

    searchTerm = searchTerm.Trim().ToLower();

    var books = await _context.Books
        .Where(b => b.Title.ToLower().Contains(searchTerm) || b.Author.ToLower().Contains(searchTerm))
        .Include(b => b.Categories) // Optionally include categories
        .ToListAsync(cancellationToken);

    return books;
}


}
