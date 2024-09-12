using Microsoft.EntityFrameworkCore;
using BookStore.Context;
using BookStore.Services.Interfaces;
using BookStore.Repositories.Interfaces;
using BookStore.Models;
using BookStore.HandleResponse;
using BookStore.Contracts;
using BookStore.DTOs;
using BookStore.Infrastructure;
using AutoMapper;

namespace BookStore.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ImageService _imageService;
        private readonly ILogger<BookService> _logger;
        private readonly ICategoryRepository _categoryRepository;

    public BookService(IBookRepository bookRepository, IMapper mapper, ImageService imageService, ILogger<BookService> logger, ICategoryRepository categoryRepository)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
        _logger = logger;
        _imageService = imageService;
        _categoryRepository = categoryRepository; 
    }

    //create book to save on database
    public async Task<Response> CreateBookAsync(CreateBookRequest request, CancellationToken cancellationToken)
    {
        try{

            bool isAvailable = await _bookRepository.AnyAsync(request.ISBN, cancellationToken);
            if (isAvailable)
                return new Response("A book with this ISBN found", 409);
        }
        catch{
            return new Response("Internal Server Error", 500);
        }

        try
        {
        string coverImageUrl = null;

        // Check if the request contains a file to be uploaded
        if (request.ImageFile != null)
        {
            // Upload the cover image to Cloudinary and get the URL
            PhotoUploadResult imageResult = await _imageService.UploadImageAsync(request.ImageFile);
            coverImageUrl = imageResult.PhotoUrl;
        }
        else
        {
            return new Response("Cover image file is required", 400); // Ensure that an image file is provided
        }

        // Create a new Book instance
        var book = new Book
        {
            Title = request.Title,
            Author = request.Author,
            Description = request.Description,
            Price = request.Price,
            ISBN = request.ISBN,
            Language = request.Language,
            PublishDate = request.PublishDate,
            CoverImageUrl = coverImageUrl // Set the cover image URL obtained from the upload
        };

                // Check and add categories
        foreach (var categoryName in request.Categories)
        {
            var category = await _categoryRepository.GetCategoryByNameAsync(categoryName, cancellationToken);

            if (category == null)
            {
                category = new Category { Name = categoryName };
                category = await _categoryRepository.AddCategoryAsync(category, cancellationToken);
            }

            book.Categories.Add(category);
        }

        var addedBook = await _bookRepository.AddNewBook(book);
                
        BookResponseDTO bookDTO = _mapper.Map<BookResponseDTO>(addedBook);

        return new Response("Book created", 200, bookDTO);
        }

        catch(Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the book.");
            return new Response("Internal Server Error", 500);
        }

    }

    //Retrieve list of all books
    public async Task<Response> GetAllAsync(CancellationToken cancellationToken)
    {
    try
    {
        var books = await _bookRepository.GetAllAsync(cancellationToken);
        var bookDtos = _mapper.Map<IEnumerable<BookResponseDTO>>(books);

        return new Response("Books retrieved successfully", 200, bookDtos);
    }
    catch (Exception ex)
    {
        // Log the exception (optional)
        // _logger.LogError(ex, "An error occurred while retrieving books.");

        return new Response($"An error occurred: {ex.Message}", 500);
    }

    }

    //Retrieve list of Books by Category
    public async Task<Response> GetByCategoryAsync(List<string> categories, CancellationToken cancellationToken)
{
    try
    {
         _logger.LogInformation("on service "+ string.Join(", ", categories));
        var books = await _bookRepository.GetBooksByCategoryAsync(categories, cancellationToken);

        if (!books.Any())
        {
            return new Response("No books found for the specified category", 404);
        }

        var bookDtos = _mapper.Map<IEnumerable<BookResponseDTO>>(books);
        return new Response("Books retrieved successfully", 200, bookDtos);
    }
    catch (Exception ex)
    {
        // Log the exception (optional)
        // _logger.LogError(ex, "An error occurred while retrieving books by category.");

        return new Response($"An error occurred: {ex.Message}", 500);
    }
}
    

    //Retrieve list of categories
public async Task<Response> GetAllCategoriesAsync(CancellationToken cancellationToken)
{
    try
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);

        if (!categories.Any())
        {
            return new Response("No categories found", 404);
        }

        var categoryDtos = _mapper.Map<IEnumerable<CategoryResponseDTO>>(categories);
        return new Response("Categories retrieved successfully", 200, categoryDtos);
    }
    catch (Exception ex)
    {
        // Log the exception (optional)
        return new Response($"An error occurred: {ex.Message}", 500);
    }
}

//Retrieve book by ID 
public async Task<Response> GetByIdAsync(Guid id, CancellationToken cancellationToken)
{
    try
    {
        var book = await _bookRepository.GetByIdAsync(id, cancellationToken);

        if (book == null)
        {
            return new Response("Book not found", 404);
        }

        var bookDto = _mapper.Map<BookResponseDTO>(book);
        return new Response("Book retrieved successfully", 200, bookDto);
    }
    catch (Exception ex)
    {
        // Log the exception (optional)
        return new Response($"An error occurred: {ex.Message}", 500);
    }

}

//Update book
public async Task<Response> UpdateBookAsync(Guid id, UpdateBookRequest updateBookDto, CancellationToken cancellationToken)
{
    try
    {
        var book = await _bookRepository.GetByIdAsync(id, cancellationToken);

        if (book == null)
        {
            return new Response("Book not found", 404);
        }

        // Update the book properties
        book.Title = updateBookDto.Title;
        book.Author = updateBookDto.Author;
        book.Description = updateBookDto.Description;
        book.Price = updateBookDto.Price;
        book.ISBN = updateBookDto.ISBN;

        // Handle cover image update
        if (updateBookDto.ImageFile != null && updateBookDto.ImageFile.Length > 0)
        {
            // Upload the image to Cloudinary (or any cloud storage service)
            PhotoUploadResult uploadResult = await _imageService.UploadImageAsync(updateBookDto.ImageFile);

            if (uploadResult == null)
            {
                return new Response("Failed to upload the cover image", 500);
            }

            // Update the CoverImageUrl property with the new image URL
            book.CoverImageUrl = uploadResult.PhotoUrl;
        }
        else if (!string.IsNullOrEmpty(updateBookDto.ImageUrl))
        {
            // If no new image is uploaded but a URL is provided, use the existing URL
            book.CoverImageUrl = updateBookDto.ImageUrl;
        }

        // Handle category updates
        if (updateBookDto.Categories != null && updateBookDto.Categories.Any())
        {
            book.Categories.Clear(); // Clear current categories

            foreach (var categoryName in updateBookDto.Categories)
            {
                var category = await _categoryRepository.GetCategoryByNameAsync(categoryName, cancellationToken);

                if (category == null)
                {
                    category = new Category { Name = categoryName };
                    category = await _categoryRepository.AddCategoryAsync(category, cancellationToken);
                }

                book.Categories.Add(category);
            }
        }


        // Save changes to the database
        await _bookRepository.UpdateAsync(book, cancellationToken);

        var updatedBookDto = _mapper.Map<BookResponseDTO>(book);
        return new Response("Book updated successfully", 200, updatedBookDto);
    }
    catch (Exception ex)
    {
        // Log the exception (optional)
        return new Response($"An error occurred: {ex.Message}", 500);
    }
}

//delete book by Id
public async Task<Response> DeleteBookAsync(Guid id, CancellationToken cancellationToken)
{
    var book = await _bookRepository.GetByIdAsync(id, cancellationToken);

    if (book == null)
    {
        return new Response("Book not found", 404);
    }

    try
    {
        await _bookRepository.DeleteAsync(book, cancellationToken);
        return new Response("Book deleted successfully", 200);
    }
    catch (Exception ex)
    {
        return new Response($"An error occurred: {ex.Message}", 500);
    }
}

// Search books by title or author
public async Task<Response> SearchBooksAsync(string searchTerm, CancellationToken cancellationToken)
{
    try
    {
        var books = await _bookRepository.SearchBooksAsync(searchTerm, cancellationToken);

        if (!books.Any())
        {
            return new Response("No books found for the specified search term", 404);
        }

        var bookDtos = _mapper.Map<IEnumerable<BookResponseDTO>>(books);
        return new Response("Books retrieved successfully", 200, bookDtos);
    }
    catch (Exception ex)
    {
        return new Response($"An error occurred: {ex.Message}", 500);
    }
}



}
}

