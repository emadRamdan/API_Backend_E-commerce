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
        [Authorize]
        public ActionResult<List<ProductDto>> GetAll()
        {
          var Products =   _ProductManger.GetAll();
            return Products.ToList();
        }

        [HttpPost]
        [Authorize(Policy = "EmployeeOnly")]
        [Route("AddProduct")]
        public ActionResult AddProduct(AddProductDto product)
        {
            _ProductManger.Add(product);
            return Created();
        }
    }
}
