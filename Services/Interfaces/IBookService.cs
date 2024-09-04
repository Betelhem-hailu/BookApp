using BookStore.HandleResponse;
using BookStore.Contracts;

namespace BookStore.Services.Interfaces {
public interface IBookService
{
        Task<Response> CreateBookAsync(CreateBookRequest request, CancellationToken cancellationToken);
        Task<Response> GetAllAsync(CancellationToken cancellationToken);
        Task<Response> GetByCategoryAsync(string category, CancellationToken cancellationToken);
        Task<Response> GetAllCategoriesAsync(CancellationToken cancellationToken);
        Task<Response> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Response> UpdateBookAsync(Guid id, UpdateBookRequest updateBookDto, CancellationToken cancellationToken);
        Task<Response> DeleteBookAsync(Guid id, CancellationToken cancellationToken);
}
}