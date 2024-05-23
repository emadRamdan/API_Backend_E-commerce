using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplicationProject.DBL.DTOS;
using WebApplicationProject.DBL.MangersContainers.OrderMangerContainer;

namespace WebApplicationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderManger _OrderManger;

        public OrdersController(IOrderManger orderManger) {
            _OrderManger = orderManger;
                
                
         }

        [Authorize(Policy = "ClientOnly")]
        [Route("CreatOrder")]
        [HttpPost]
        public ActionResult CreatOrder(decimal TotalPrice) {

            var UserId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            var CartID = User.Claims.ToList().FirstOrDefault(claim => claim.Type == "CartID").Value;


            var orderDto = new CreatOrderDto { TotalPrice = TotalPrice , CartId = CartID, UserId = UserId };
            _OrderManger.CreateOrder(orderDto);
            return Created(); 
        }

        [Authorize(Policy = "ClientOrEmployee")]
        [Route("UserCancelOrder")]
        [HttpPost]
        public ActionResult CancelOrder(int orderID) {

       
            _OrderManger.ChangeOrderStatus(orderID, "Canceled");
            return Ok("order Canceled");
        }


        [Route("ALlOrders")]
        [Authorize(Policy = "EmployeeOnly")]
        [HttpGet]
        public ActionResult<List<OrderDto>> GetAllOrders() {
            
          var orders =   _OrderManger.GetAllOrders().ToList();
            return orders; 
        }

        [Route("UserALlOrders")]
        [Authorize(Policy = "ClientOnly")]
        [HttpGet]
        public ActionResult<List<OrderDto>> GetUserOrders()
        {

            var USerID = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value; 
            var orders = _OrderManger.GetUserOrders(USerID).ToList();
            return orders;
        }


    }
}
