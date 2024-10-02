using KTOPWebAPI.Entities;
using KTOPWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KTOPWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly KtopdbContext ktopdbContext;

        public CategoryController(KtopdbContext ktopdbContext)
        {
            this.ktopdbContext = ktopdbContext;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllCategories()
        {
            try
            {
                return Ok(ktopdbContext.Categories.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{categoryId}")]
        [Authorize]
        public IActionResult GetCategoryById(int categoryId)
        {
            try
            {
                var category = ktopdbContext.Categories.Find(categoryId);
                if (category == null) return NotFound("Category not found");
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddCategory([FromBody] CategoryModel categoryModel)
        {
            try
            {
                var existingCategory = ktopdbContext.Categories
                    .FirstOrDefault(c => c.CategoryName == categoryModel.CategoryName);
                if (existingCategory != null) return Conflict("Category already exists");

                Category newCategory = new()
                {
                    CategoryName = categoryModel.CategoryName
                };
                ktopdbContext.Categories.Add(newCategory);
                ktopdbContext.SaveChanges();
                return Ok("Category added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{categoryId}")]
        [Authorize]
        public IActionResult EditCategory(int categoryId, [FromBody] CategoryModel categoryModel)
        {
            try
            {
                var editCategory = ktopdbContext.Categories.Find(categoryId);
                if (editCategory == null) return NotFound("Category not found");

                var existingCategory = ktopdbContext.Categories
                    .FirstOrDefault(c => c.CategoryName == categoryModel.CategoryName && c.CategoryId != categoryId);
                if (existingCategory != null) return Conflict("Category already exists");

                editCategory.CategoryName = categoryModel.CategoryName;
                ktopdbContext.SaveChanges();
                return Ok("Category updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{categoryId}")]
        [Authorize]
        public IActionResult DeleteCategory(int categoryId)
        {
            try
            {
                var deleteCategory = ktopdbContext.Categories.Find(categoryId);
                if (deleteCategory == null) return NotFound("Category not found");

                if (deleteCategory.CategoryName == "Inne") return BadRequest("Cannot delete the 'Inne' category.");

                var relatedProducts = ktopdbContext.Products.Where(p => p.CategoryId == categoryId).ToList();
                if (relatedProducts.Any())
                {
                    var otherCategory = ktopdbContext.Categories.FirstOrDefault(c => c.CategoryName == "Inne");
                    if (otherCategory == null)
                    {
                        otherCategory = new Category { CategoryName = "Inne" };
                        ktopdbContext.Categories.Add(otherCategory);
                        ktopdbContext.SaveChanges();
                    }

                    foreach (var product in relatedProducts)
                    {
                        product.CategoryId = otherCategory.CategoryId;
                    }
                }

                ktopdbContext.Categories.Remove(deleteCategory);
                ktopdbContext.SaveChanges();
                return Ok("Category deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{categoryName}")]
        [Authorize]
        public IActionResult SearchCategoryByName(string categoryName)
        {
            try
            {
                var searchCategoryName = ktopdbContext.Categories.Where(c => c.CategoryName.Contains(categoryName));
                if (searchCategoryName == null) return NotFound("Category not found");
                return Ok(searchCategoryName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
