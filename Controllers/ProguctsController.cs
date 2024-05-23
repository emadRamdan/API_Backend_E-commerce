using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationProject.DBL.DTOS;
using WebApplicationProject.DBL.MangersContainers.ProductMangerConainer;

namespace WebApplicationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProguctsController : ControllerBase
    {
        private IProductManger _ProductManger;

        public ProguctsController(IProductManger productManger)
        { 
            _ProductManger = productManger;
        }


        [HttpGet]
        [Route("AllProducts")]
        //[Authorize]
        public ActionResult<List<ProductDto>> GetAll(int? CategoryId, string? ProductName)
        {
            if (CategoryId is not null)
            {
                var Products = _ProductManger.GetAllByCategory((int)CategoryId);
                return Products.ToList();
            }
            else if (!string.IsNullOrEmpty(ProductName))
            {
                var Products = _ProductManger.GetAllByName(ProductName);
                return Products.ToList();
            }
            else
            {
                var Products = _ProductManger.GetAll();
                return Products.ToList();
            }
          
        }

        [HttpPost]
        [Authorize(Policy = "EmployeeOnly")]
        [Route("AddProduct")]
        public async Task<ActionResult> AddProduct([FromForm] AddProductDto product)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

         var result = await _ProductManger.Add(product);
        
            if (result.SuccessFlag == false )
            {
                return BadRequest(result.Message);
            }
            return Created();
        }
    }
}
