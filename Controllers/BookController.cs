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

    // [HttpGet]
    // public IActionResult GetListOfBooks()
    // {
    //     try
    //         {
    //             var book = await _bookService.GetAllAsync();
    //             if (book == null || !book.Any())
    //             {
    //                 return Ok(new { message = "No Book Items  found" });
    //             }
    //             return Ok(new { message = "Successfully retrieved all book posts", data = book });

    //         }
    //         catch (Exception ex)
    //         {
    //             return StatusCode(500, new { message = "An error occurred while retrieving all book it posts", error = ex.Message });


    //         }
    //     return Ok("Customer content");
    // }
    // [HttpGet("{id:guid}")]
    // public IActionResult GetBook()
    // {
    //      try
    //         {
    //             var book = await _bookServices.GetAllAsync();
    //             if (book == null || !book.Any())
    //             {
    //                 return Ok(new { message = "No Book Items  found" });
    //             }
    //             return Ok(new { message = "Successfully retrieved all book posts", data = book });

    //         }
    //         catch (Exception ex)
    //         {
    //             return StatusCode(500, new { message = "An error occurred while retrieving all book it posts", error = ex.Message });


    //         }
    //     return Ok("Customer content");
    // }

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
