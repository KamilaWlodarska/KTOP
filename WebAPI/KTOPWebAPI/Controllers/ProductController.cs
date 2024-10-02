using KTOPWebAPI.Entities;
using KTOPWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KTOPWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly KtopdbContext ktopdbContext;
        public ProductController(KtopdbContext ktopdbContext)
        {
            this.ktopdbContext = ktopdbContext;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllProducts()
        {
            try
            {
                return Ok(ktopdbContext.Products.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{productId}")]
        [Authorize]
        public IActionResult GetProductById(int productId)
        {
            try
            {
                var product = ktopdbContext.Products.Find(productId);
                if (product == null) return NotFound("Product not found");
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddProduct([FromBody] ProductModel productModel)
        {
            try
            {
                var category = ktopdbContext.Categories.Find(productModel.CategoryId);
                if (category == null) return NotFound("Category not found");

                var home = ktopdbContext.Homes.Find(productModel.HomeId);
                if (home == null) return NotFound("Home not found");

                Product newProduct = new()
                {
                    ProductName = productModel.ProductName,
                    CategoryId = productModel.CategoryId,
                    PurchaseDate = DateTime.Now.Date,
                    ExpiryDate = productModel.ExpiryDate,
                    OpenDate = null,
                    HomeId = productModel.HomeId
                };
                ktopdbContext.Products.Add(newProduct);
                ktopdbContext.SaveChanges();
                return Ok("Product added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{productId}")]
        [Authorize]
        public IActionResult EditProduct(int productId, [FromBody] ProductModel productModel)
        {
            try
            {
                var editProduct = ktopdbContext.Products.Find(productId);
                if (editProduct == null) return NotFound("Product not found");

                var category = ktopdbContext.Categories.Find(productModel.CategoryId);
                if (category == null) return NotFound("Category not found");

                var home = ktopdbContext.Homes.Find(productModel.HomeId);
                if (home == null) return NotFound("Home not found");

                editProduct.ProductName = productModel.ProductName;
                editProduct.CategoryId = productModel.CategoryId;
                editProduct.PurchaseDate = productModel.PurchaseDate;
                editProduct.ExpiryDate = productModel.ExpiryDate;
                editProduct.OpenDate = productModel.OpenDate;
                editProduct.HomeId = productModel.HomeId;

                ktopdbContext.SaveChanges();
                return Ok("Product updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{productId}")]
        [Authorize]
        public IActionResult DeleteProduct(int productId)
        {
            try
            {
                var deleteProduct = ktopdbContext.Products.Find(productId);
                if (deleteProduct == null) return NotFound("Product not found");
                ktopdbContext.Products.Remove(deleteProduct);
                ktopdbContext.SaveChanges();
                return Ok("Product deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{productName}")]
        [Authorize]
        public IActionResult SearchProductByName(string productName)
        {
            try
            {
                var searchProductName = ktopdbContext.Products
                    .Where(p => p.ProductName.Contains(productName));
                if (searchProductName == null) return NotFound("Product not found");
                return Ok(searchProductName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}/{productName}")]
        [Authorize]
        public IActionResult SearchUserProductByName(int userId, string productName)
        {
            try
            {
                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");

                var userHomeIds = ktopdbContext.UsersHomes
                    .Where(uh => uh.UserId == userId).Select(uh => uh.HomeId).ToList();

                var searchProduct = ktopdbContext.Products.Where(p => userHomeIds
                    .Contains(p.HomeId) && p.ProductName.Contains(productName)).ToList();

                if (searchProduct == null || searchProduct.Count == 0) return NotFound("Product not found");
                return Ok(searchProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        [Authorize]
        public IActionResult GetAllUserProducts(int userId)
        {
            try
            {
                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");

                var userProducts = ktopdbContext.Products.Where(p => ktopdbContext.UsersHomes
                    .Any(uh => uh.UserId == userId && uh.HomeId == p.HomeId)).ToList();

                return Ok(userProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        [Authorize]
        public IActionResult GetAllUserProductsExpirationDates(int userId)
        {
            try
            {
                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");

                var userProducts = ktopdbContext.Products
                    .Where(p => ktopdbContext.UsersHomes
                    .Any(uh => uh.UserId == userId && uh.HomeId == p.HomeId))
                    .ToList();

                var expiryDates = userProducts.Select(p => p.ExpiryDate).ToList();
                return Ok(expiryDates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}/{expireDate}")]
        [Authorize]
        public IActionResult GetAllUserProductsByExpirationDate(int userId, string expireDate)
        {
            try
            {
                if (!DateTime.TryParse(expireDate, out DateTime expirationDate))
                {
                    return BadRequest("Invalid expiration date format");
                }

                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");

                var userProducts = ktopdbContext.Products
                    .Where(p => ktopdbContext.UsersHomes
                    .Any(uh => uh.UserId == userId && uh.HomeId == p.HomeId)
                     && p.ExpiryDate.Date == expirationDate.Date).ToList();

                return Ok(userProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}/{homeId}")]
        [Authorize]
        public IActionResult GetAllUserProductsByHome(int userId, int homeId)
        {
            try
            {
                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");

                var userProductsByHome = ktopdbContext.Products.Where(p => ktopdbContext.UsersHomes
                    .Any(uh => uh.UserId == userId && uh.HomeId == homeId)
                    && p.HomeId == homeId).ToList();

                return Ok(userProductsByHome);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        [Authorize]
        public IActionResult GetAllUserProductsWithOpenDate(int userId)
        {
            try
            {
                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");

                var userProducts = ktopdbContext.Products.Where(p => ktopdbContext.UsersHomes
                    .Any(uh => uh.UserId == userId && uh.HomeId == p.HomeId)
                    && p.OpenDate != null).OrderBy(p => p.OpenDate).ToList();

                return Ok(userProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}/{categoryId}")]
        [Authorize]
        public IActionResult GetAllUserProductsByCategory(int userId, int categoryId)
        {
            try
            {
                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");

                var userProductsByCategory = ktopdbContext.Products.Where(p => ktopdbContext.UsersHomes
                    .Any(uh => uh.UserId == userId && uh.HomeId == p.HomeId)
                    && p.CategoryId == categoryId).ToList();

                return Ok(userProductsByCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        [Authorize]
        public IActionResult GetAllUserExpiredProducts(int userId)
        {
            try
            {
                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");

                var userExpiredProducts = ktopdbContext.Products.Where(p => ktopdbContext.UsersHomes
                    .Any(uh => uh.UserId == userId && uh.HomeId == p.HomeId)
                    && p.ExpiryDate < DateTime.Now).ToList();

                return Ok(userExpiredProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        [Authorize]
        public IActionResult GetUserProductsExpiringThisWeek(int userId)
        {
            try
            {
                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");

                var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                var endOfWeek = startOfWeek.AddDays(6);

                var userProductsExpiringThisWeek = ktopdbContext.Products
                    .Where(p => ktopdbContext.UsersHomes.Any(uh => uh.UserId == userId && uh.HomeId == p.HomeId)
                        && p.ExpiryDate >= startOfWeek && p.ExpiryDate <= endOfWeek).ToList();

                return Ok(userProductsExpiringThisWeek);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        [Authorize]
        public IActionResult GetUserProductsSortedByPurchaseDateAsc(int userId)
        {
            try
            {
                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");

                var userProductsSorted = ktopdbContext.Products
                    .Where(p => ktopdbContext.UsersHomes
                    .Any(uh => uh.UserId == userId && uh.HomeId == p.HomeId))
                    .OrderBy(p => p.PurchaseDate).ToList();

                return Ok(userProductsSorted);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        [Authorize]
        public IActionResult GetUserProductsSortedByPurchaseDateDsc(int userId)
        {
            try
            {
                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");

                var userProductsSorted = ktopdbContext.Products
                    .Where(p => ktopdbContext.UsersHomes
                    .Any(uh => uh.UserId == userId && uh.HomeId == p.HomeId))
                    .OrderByDescending(p => p.PurchaseDate).ToList();

                return Ok(userProductsSorted);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        [Authorize]
        public IActionResult GetUserProductsSortedByOpenDateAsc(int userId)
        {
            try
            {
                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");

                var userProductsSorted = ktopdbContext.Products
                    .Where(p => ktopdbContext.UsersHomes
                    .Any(uh => uh.UserId == userId && uh.HomeId == p.HomeId))
                    .OrderBy(p => p.OpenDate).ToList();

                return Ok(userProductsSorted);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        [Authorize]
        public IActionResult GetUserProductsSortedByOpenDateDsc(int userId)
        {
            try
            {
                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");

                var userProductsSorted = ktopdbContext.Products
                    .Where(p => ktopdbContext.UsersHomes
                    .Any(uh => uh.UserId == userId && uh.HomeId == p.HomeId))
                    .OrderByDescending(p => p.OpenDate).ToList();

                return Ok(userProductsSorted);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        [Authorize]
        public IActionResult GetUserProductsSortedByExpiryDateAsc(int userId)
        {
            try
            {
                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");

                var userProductsSorted = ktopdbContext.Products
                    .Where(p => ktopdbContext.UsersHomes
                    .Any(uh => uh.UserId == userId && uh.HomeId == p.HomeId))
                    .OrderBy(p => p.ExpiryDate).ToList();

                return Ok(userProductsSorted);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        [Authorize]
        public IActionResult GetUserProductsSortedByExpiryDateDsc(int userId)
        {
            try
            {
                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");

                var userProductsSorted = ktopdbContext.Products
                    .Where(p => ktopdbContext.UsersHomes
                    .Any(uh => uh.UserId == userId && uh.HomeId == p.HomeId))
                    .OrderByDescending(p => p.ExpiryDate).ToList();

                return Ok(userProductsSorted);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
