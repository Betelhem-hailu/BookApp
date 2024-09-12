using BookStore.Contracts;
namespace BookStore.Repositories.Interfaces {
public interface IBookRepository
{
    Task<Book> AddNewBook(Book request);
    Task<Book?> GetBookByISBNAsync(string ISBN, CancellationToken cancellationToken);
    Task<bool> AnyAsync(string ISBN, CancellationToken cancellationToken);

    Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Book>> GetBooksByCategoryAsync(List<string> categories, CancellationToken cancellationToken);
    Task<Book> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task UpdateAsync(Book book, CancellationToken cancellationToken);
    Task DeleteAsync(Book book, CancellationToken cancellationToken);

    Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm, CancellationToken cancellationToken);
}
}