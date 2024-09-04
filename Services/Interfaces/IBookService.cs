using BookStore.HandleResponse;
using BookStore.Contracts;

namespace BookStore.Services.Interfaces {
public interface IBookService
{
//    Task<IEnumerable<Book>> GetAllBookAsync();
//         Task<Book> GetBookByIdAsync(Guid id);
         Task<Response> CreateBookAsync(CreateBookRequest request, CancellationToken cancellationToken);
        // Task UpdateBookAsync(Guid id, UpdateBookRequest request);
        // Task DeleteBookAsync(Guid id);
}
}