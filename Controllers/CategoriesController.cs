using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationProject.DAL.models;
using WebApplicationProject.DBL.DTOS;
using WebApplicationProject.DBL.MangersContainers.CategoryMangerContainer;
using static System.Net.Mime.MediaTypeNames;

namespace WebApplicationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ICategoryManger _CategoryManger;

        public CategoriesController(ICategoryManger categoryManger)
        {
            _CategoryManger = categoryManger;
        }

        [HttpGet]
        [Route("AllCategories")]
        public ActionResult<List<CategoryDto>> GetAll()
        {
           var categories =  _CategoryManger.GetAll();

            return categories.ToList();
        }

        [HttpPost]
        [Route("AddCategory")]
        //[Authorize(Policy = "EmployeeOnly")]
        public async Task<ActionResult> AddCategory([FromForm] AddCategoryDto category)
        {
            try
            {
                if(category == null || category.CategoryImageFile == null || category.CategoryImageFile.Length == 0)
                {
                    return BadRequest("No image file uploaded");
                }

                if (!category.CategoryImageFile.ContentType.StartsWith("image/"))
                {
                    return BadRequest("Invalid file type. Only image files are allowed.");
                }

                var UploadeFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploades");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(category.CategoryImageFile.FileName);
                var FilePath = Path.Combine(UploadeFolderPath, fileName);

                using (var stream = new FileStream(FilePath, FileMode.Create))
                {
                    await category.CategoryImageFile.CopyToAsync(stream);
                }

                var categoryDto = new CategoryDto { CategoryName =  category.CategoryName  , CategoryImage = Path.Combine("/Uploades" , fileName) };
                _CategoryManger.Add(categoryDto);
                return Created();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");

            }

           
        }
    }
}
