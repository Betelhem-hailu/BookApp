using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookStore.Services.Interfaces;
using BookStore.Contracts;
using BookStore.Data.Cart;
using AutoMapper;
using BookStore.DTOs;

// [ApiController]
// [Route("api/[controller]")]
// public class CartController : ControllerBase
// {
//     private readonly ICartService _cartService;

//     public CartController(ICartService cartService)
//     {
//         _cartService = cartService;
//     }

//     [HttpGet]
//     public async Task<IActionResult> GetCart(CancellationToken cancellationToken)
//     {
//         var userId = GetUserId();  // Assuming you have a method to get the user ID
//         var cart = await _cartService.GetCartAsync(userId, cancellationToken);
//         return Ok(cart);
//     }

//     [HttpPost("add")]
//     [Authorize(Policy = "CustomerOnly")]
//     public async Task<IActionResult> AddToCart(Guid bookId, int quantity, CancellationToken cancellationToken)
//     {
//         var userId = GetUserId();  // Assuming you have a method to get the user ID
//         await _cartService.AddToCartAsync(userId, bookId, quantity, cancellationToken);
//         return Ok();
//     }

//     [HttpPost("remove")]
//     [Authorize(Policy = "CustomerOnly")]
//     public async Task<IActionResult> RemoveFromCart(Guid bookId, CancellationToken cancellationToken)
//     {
//         var userId = GetUserId();
//         await _cartService.RemoveFromCartAsync(userId, bookId, cancellationToken);
//         return Ok();
//     }

//     [HttpPost("update")]
//     [Authorize(Policy = "CustomerOnly")]
//     public async Task<IActionResult> UpdateCart(Guid bookId, int quantity, CancellationToken cancellationToken)
//     {
//         var userId = GetUserId();
//         await _cartService.UpdateCartAsync(userId, bookId, quantity, cancellationToken);
//         return Ok();
//     }

//     // [HttpPost("checkout")]
//     // public async Task<IActionResult> Checkout(CancellationToken cancellationToken)
//     // {
//     //     var userId = GetUserId();
//     //     await _cartService.CheckoutAsync(userId, cancellationToken);
//     //     return Ok("Order completed.");
//     // }

    // private Guid GetUserId()
    // {
    //     // This method should retrieve the logged-in user's ID
    //     return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    // }
// }

namespace BookStore.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ShoppingCart _shoppingCart;
        private readonly IOrderService _orderService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public CartController(IMapper mapper, IBookService bookService, IOrderService orderService, IAuthService authService, ShoppingCart shoppingCart, IServiceProvider serviceProvider)
        {
            _bookService = bookService;
            _orderService = orderService;
            _authService = authService;
            _shoppingCart = ShoppingCart.GetCart(serviceProvider);
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();
            var items = _shoppingCart.GetShoppingCartItems(userId);
            var shoppingCartItemsDTO = items.Select(item => new ShoppingCartItemDTO
            {
                ShoppingCartItemId = item.ShoppingCartItemId,
                BookId = item.Book.BookId,
                BookTitle = item.Book.Title,
                CoverImageUrl = item.Book.CoverImageUrl,
                Quantity = item.Quantity,
                UnitPrice = item.Book.Price
            }).ToList();


            var shoppingCartViewModel = new ShoppingCartDTO
            {
                Items = shoppingCartItemsDTO,
                Total = _shoppingCart.GetShoppingCartTotal(userId)
            };
            return Ok(shoppingCartViewModel);
        }

        [HttpPost("add")]
        [Authorize(Policy = "CustomerOnly")]
        public async Task<IActionResult> AddToCart(Guid bookId, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            var book = await _bookService.GetByIdAsync(bookId, cancellationToken);

            if(book.Book != null)
            {
                _shoppingCart.AddItemToCart(book.Book, userId);
            return Ok(new { success = true, message = "Book added to cart successfully" });
            }
            else
            {
                return NotFound(new { success = false, message = "Book not found" });
            }
        }

        [HttpDelete("remove")]
        [Authorize(Policy = "CustomerOnly")]
        public async Task<IActionResult> RemoveItemFromCart(Guid bookId, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            _shoppingCart.RemoveItemFromCart(bookId, userId);
            return Ok(new { success = true, message = "Book removed from cart successfully" });
        }

        [HttpDelete("Delete")]
        [Authorize(Policy = "CustomerOnly")]
        public async Task<IActionResult> RemoveFromCart(Guid shoppingCartItemId, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            _shoppingCart.RemoveFromCart(shoppingCartItemId, userId);
            return Ok(new { success = true, message = "Cart removed from cart successfully" });
        }

        // [HttpPost("update")]
        // [Authorize(Policy = "CustomerOnly")]
        // public async Task<IActionResult> UpdateCart(Guid bookId, int quantity, CancellationToken cancellationToken)
        // {
        //     var userId = GetUserId();
        //     await _cartService.UpdateCartAsync(userId, bookId, quantity, cancellationToken);
        //     return Ok();
        // }

        [HttpGet("order")]
        public async Task<IActionResult> GetOrders(CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            var orders = await _orderService.GetOrderByUserAsync(userId, cancellationToken);
            
            return Ok(orders);
        } 

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            var items = _shoppingCart.GetShoppingCartItems(userId);
            var user = _authService.GetByIdAsync(userId);

            await _orderService.StoreOrderAsync(items, userId, user.Email, cancellationToken);
            await _shoppingCart.ClearShoppingCartAsync(userId);

            return Ok("Order completed.");
        }


        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}