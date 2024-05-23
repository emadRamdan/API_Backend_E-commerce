using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationProject.DBL.DTOS;
using WebApplicationProject.DBL.MangersContainers.CartMangerContainer;

namespace WebApplicationProject.Controllers
{

    // to do ad auth 
    [Authorize(Policy = "ClientOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartManger _cartManger;

        public CartController( ICartManger cartManger)
        {
            _cartManger = cartManger;
        }

       

        [HttpGet]
        [Route("GetCart")]
        
        public ActionResult<ShoppingCartDto> GetCart()
        {
            var userCartID = User.Claims.ToList().FirstOrDefault(claim => claim.Type == "CartID").Value;
            if (userCartID != null)
            {
                var cartDto = _cartManger.GetCart(userCartID);
                return cartDto;
            }
            else
            {
                return BadRequest(); 
            }
           
        }



        [HttpPost]
        [Route("AddToCart")]
        [Authorize(Policy = "ClientOnly")]

        public ActionResult AddToCart( int ProductId)
        {
            var userCartID = User.Claims.ToList().FirstOrDefault(claim => claim.Type == "CartID").Value;

            _cartManger.AddToCart(userCartID, ProductId);
            return Ok();
        }

        [Authorize(Policy = "ClientOnly")]
        [HttpPost]
        [Route("RemoveFromCart")]
        public ActionResult RemoveFromCart(int cartId, int ItemId)
        {
            _cartManger.RemoveFromCart(cartId, ItemId);
            return Ok();
        }


        [HttpPut]
        [Route("EditItemQuanity")]
        public ActionResult EditItemQuanity(int cartId, int itemId, int Quanity)
        {
            _cartManger.EditItemQuantity(cartId, itemId, Quanity);
            return Ok();
        }
    }
}
