using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookStore.Services.Interfaces;
using BookStore.Contracts;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<IActionResult> GetListOfBooks(CancellationToken cancellation)
    {
        var response = await _bookService.GetAllAsync(cancellation);
            if (response.Status == 500) return StatusCode(500, response);

            return Ok(response);
    }

    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetBooksByCategory(string category, CancellationToken cancellationToken)
    {
        var response = await _bookService.GetByCategoryAsync(category, cancellationToken);

        if (response.Status == 200) return Ok(response);
        if (response.Status == 404) return NotFound(response);
        else
        {
            return StatusCode(500, response);
        }
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
    {
        var response = await _bookService.GetAllCategoriesAsync(cancellationToken);

        if (response.Status == 200) return Ok(response);
        if (response.Status == 404) return NotFound(response);
        else
        {
            return StatusCode(500, response);
        }
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBook(Guid id, CancellationToken cancellationToken)
    {
        var response = await _bookService.GetByIdAsync(id, cancellationToken);
        if (response.Status == 200) return Ok(response);
        if (response.Status == 404) return NotFound(response);
        else
        {
            return StatusCode(500, response);
        }
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> CreateBook([FromForm] CreateBookRequest bookRequest, CancellationToken cancellation)
    {
        var response = await _bookService.CreateBookAsync(bookRequest, cancellation);

            if (response.Status == 409) return Conflict(response);
            if (response.Status == 400) return BadRequest(response);
            if (response.Status == 500) return StatusCode(500, response);

            return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult EditBook()
    {
        // This route is protected and can only be accessed by users with the "Admin" role
        return Ok("Admin content");
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult DeleteBook()
    {
        // This route is protected and can only be accessed by users with the "Admin" role
        return Ok("Admin content");
    }
}
