// Services/CartService.cs
using dotnetapp.Models;

namespace dotnetapp.Services
{
    public interface CartService
    {
        // Cart addCart(Cart cart);
        Cart updateCart(Cart updatedCart);
        Cart getCartByCustomerId(long customerId);
        List<Gift> getAllGiftsByCustomerId(long customerId);
    }
}
