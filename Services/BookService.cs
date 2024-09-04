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

    }    

}

