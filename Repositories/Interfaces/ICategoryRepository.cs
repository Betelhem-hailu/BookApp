namespace BookStore.Repositories.Interfaces {
    public interface ICategoryRepository
{
    Task<Category> GetCategoryByNameAsync(string name, CancellationToken cancellationToken);
    Task<Category> AddCategoryAsync(Category category, CancellationToken cancellationToken);
}
}