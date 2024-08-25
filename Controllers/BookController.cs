using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    [HttpGet("admin")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult GetAdminBooks()
    {
        // This route is protected and can only be accessed by users with the "Admin" role
        return Ok("Admin content");
    }

    [HttpGet("customer")]
    [Authorize(Policy = "CustomerOnly")]
    public IActionResult GetCustomerBooks()
    {
        // This route is protected and can only be accessed by users with the "Customer" role
        return Ok("Customer content");
    }
}
