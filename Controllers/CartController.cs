using Loja.Services.Interfaces;
using Loja.Dtos.Cart;
using Microsoft.AspNetCore.Mvc;

namespace Loja.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET api/cart/{sessionId} - obter ou criar carrinho pela sessão
        [HttpGet("{sessionId}")]
        public async Task<IActionResult> GetOrCreateCartBySessionId(string sessionId)
        {
            var cart = await _cartService.GetOrCreateCartBySessionIdAsync(sessionId);
            return Ok(cart);
        }

        // POST api/cart/{sessionId}/items - adicionar itens ao carrinho
        [HttpPost("{sessionId}/items")]
        public async Task<IActionResult> AddItemsToCart(string sessionId, [FromBody] List<CreateCartItemDto> items)
        {
            var cart = await _cartService.AddItemsToCartAsync(sessionId, items);
            return Ok(cart);
        }


        // DELETE api/cart/{sessionId}/items - remover itens específicos (lista de productIds no corpo)
        [HttpDelete("{sessionId}/items")]
        public async Task<IActionResult> RemoveItemsFromCart(string sessionId, [FromBody] List<int> productIds)
        {
            var cart = await _cartService.RemoveItemsFromCartAsync(sessionId, productIds);
            if (cart is null)
            {
                return NotFound();
            }
            return Ok(cart);
        }


        // PUT api/cart/{sessionId}/items/{productId}?quantity=5 - atualizar quantidade de um item
        [HttpPut("{sessionId}/items/{productId}")]
        public async Task<IActionResult> UpdateCartItemQuantity(string sessionId, int productId, [FromQuery] int quantity)
        {
            var cart = await _cartService.UpdateCartItemQuantityAsync(sessionId, productId, quantity);
            if (cart is null)
            {
                return NotFound();
            }
            return Ok(cart);
        }


        // DELETE api/cart/{sessionId}/clear - limpar carrinho inteiro
        [HttpDelete("{sessionId}/clear")]
        public async Task<IActionResult> ClearCart(string sessionId)
        {
            var cart = await _cartService.ClearCartAsync(sessionId);
            if (cart is null)
            {
                return NotFound();
            }
            return Ok(cart);
        }


        // GET api/cart/{sessionId}/items - obter carrinho existente pela sessão
        [HttpGet("{sessionId}/items")]
        public async Task<IActionResult> GetCartBySessionId(string sessionId)
        {
            var cart = await _cartService.GetCartBySessionIdAsync(sessionId);
            if (cart is null)
            {
                return NotFound();
            }
            return Ok(cart);
        }

    }
}
