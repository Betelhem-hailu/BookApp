using Microsoft.EntityFrameworkCore;
using BookStore.Context;
using BookStore.Models;


namespace BookStore.Data.Cart
{
    public class ShoppingCart
    {
        public BookDbContext _context { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public ShoppingCart(BookDbContext context, IHttpContextAccessor httpContextAccessor) {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public static ShoppingCart GetCart(IServiceProvider services)
    {
        var context = services.GetService<BookDbContext>();
        var httpContextAccessor = services.GetService<IHttpContextAccessor>();

        string cartId = httpContextAccessor.HttpContext.Session.GetString("CartId") 
            ?? Guid.NewGuid().ToString();

        httpContextAccessor.HttpContext.Session.SetString("CartId", cartId);

        return new ShoppingCart(context, httpContextAccessor) { ShoppingCartId = cartId };
    }



        public void AddItemToCart(Book book, Guid userId)
        {
            var ShoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(n => n.Book.BookId == book.BookId && 
            n.UserId == userId);

            if(ShoppingCartItem == null)
            {
            ShoppingCartItem = new ShoppingCartItem()
                {
                    ShoppingCartId = ShoppingCartId,
                    Book = book,
                    Quantity = 1,
                    UserId = userId
                };

                _context.ShoppingCartItems.Add(ShoppingCartItem);
            }
            else
            {
                ShoppingCartItem.Quantity++;
            }
            _context.SaveChanges();
        }

        public void RemoveItemFromCart(Guid bookId, Guid userId)
        {
            var ShoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(n => n.BookId == bookId && 
            n.UserId == userId);

            if(ShoppingCartItem != null)
            {
                if(ShoppingCartItem.Quantity > 1)
                {
                    ShoppingCartItem.Quantity--;
                }
                else
                {
                    _context.ShoppingCartItems.Remove(ShoppingCartItem);
                }
            }
            _context.SaveChanges();
        }

        public void RemoveFromCart(Guid shoppingCartItemId, Guid userId)
        {
            var ShoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(n => n.ShoppingCartItemId == shoppingCartItemId && 
            n.UserId == userId);

            if(ShoppingCartItem != null)
            {
                    _context.ShoppingCartItems.Remove(ShoppingCartItem);
            }
            _context.SaveChanges();
        }

        public List<ShoppingCartItem> GetShoppingCartItems(Guid userId)
        {
            return _context.ShoppingCartItems
                .Where(c => c.UserId == userId)
                .Include(n => n.Book)
                .ToList();
        }



        public decimal GetShoppingCartTotal(Guid userId) => _context.ShoppingCartItems
            .Where(n => n.UserId == userId)
            .Select(n => n.Book.Price * n.Quantity)
            .Sum();

        public async Task ClearShoppingCartAsync(Guid userId)
        {
            var items = await _context.ShoppingCartItems
            .Where(n => n.UserId == userId)
            .ToListAsync();
            _context.ShoppingCartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

    }
}