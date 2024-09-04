using BookStore.Contracts;
namespace BookStore.Repositories.Interfaces {
public interface IBookRepository
{
    Task<Book> AddNewBook(Book request);
    Task<Book?> GetBookByISBNAsync(string ISBN, CancellationToken cancellationToken);
    Task<bool> AnyAsync(string ISBN, CancellationToken cancelationToken);
}
}