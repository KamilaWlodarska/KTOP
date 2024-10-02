using KTOPWebAPI.Entities;
using KTOPWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KTOPWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly KtopdbContext ktopdbContext;

        public HomeController(KtopdbContext ktopdbContext)
        {
            this.ktopdbContext = ktopdbContext;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllHomes()
        {
            try
            {
                return Ok(ktopdbContext.Homes.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{homeId}")]
        [Authorize]
        public IActionResult GetHomeById(int homeId)
        {
            try
            {
                var home = ktopdbContext.Homes.Find(homeId);
                if (home == null) return NotFound("Home not found");
                return Ok(home);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddHome([FromBody] HomeModel homeModel)
        {
            try
            {
                var user = ktopdbContext.Users.Find(homeModel.OwnerId);
                if (user == null) return NotFound("Owner not found");

                var existingHome = ktopdbContext.Homes
                    .FirstOrDefault(h => h.HomeName == homeModel.HomeName && h.OwnerId == homeModel.OwnerId);
                if (existingHome != null) return Conflict("Home with the same name already exists for the owner");

                Home newHome = new()
                {
                    HomeName = homeModel.HomeName,
                    OwnerId = homeModel.OwnerId
                };
                ktopdbContext.Homes.Add(newHome);

                ktopdbContext.SaveChanges();
                return Ok("Home added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{homeId}")]
        [Authorize]
        public IActionResult EditHome(int homeId, [FromBody] HomeModel homeModel)
        {
            try
            {
                var editHome = ktopdbContext.Homes.Find(homeId);
                if (editHome == null) return NotFound("Home not found");

                var user = ktopdbContext.Users.Find(homeModel.OwnerId);
                if (user == null) return NotFound("Owner not found");

                var isUserAddedToHome = ktopdbContext.UsersHomes
                    .Any(uh => uh.HomeId == homeId && uh.UserId == homeModel.OwnerId);
                if (!isUserAddedToHome)
                    return BadRequest("The new owner must be a user added to this home");

                var existingHome = ktopdbContext.Homes
                    .FirstOrDefault(h => h.HomeName == homeModel.HomeName && h.OwnerId == homeModel.OwnerId && h.HomeId != homeId);
                if (existingHome != null) return Conflict("Home with the same name already exists for the owner");

                editHome.HomeName = homeModel.HomeName;
                editHome.OwnerId = homeModel.OwnerId;

                ktopdbContext.SaveChanges();
                return Ok("Home updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{homeName}")]
        [Authorize]
        public IActionResult SearchHomeByName(string homeName)
        {
            try
            {
                var searchHome = ktopdbContext.Homes.Where(p => p.HomeName.Contains(homeName));
                if (searchHome == null) return NotFound("Home not found");
                return Ok(searchHome);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}/{homeName}")]
        [Authorize]
        public IActionResult SearchUserHomeByName(int userId, string homeName)
        {
            try
            {
                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");

                var searchHome = ktopdbContext.Homes.Where(h => h.UsersHomes
                    .Any(uh => uh.UserId == userId) && h.HomeName.Contains(homeName));
                if (searchHome == null) return NotFound("Home not found");

                return Ok(searchHome);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        [Authorize]
        public IActionResult GetAllUserHomes(int userId)
        {
            try
            {
                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");

                var userHomes = ktopdbContext.Homes.Where(h => ktopdbContext.UsersHomes
                    .Any(uh => uh.UserId == userId && uh.HomeId == h.HomeId)).ToList();

                return Ok(userHomes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{userId}")]
        [Authorize]
        public IActionResult AddUserHome(int userId, [FromBody] HomeOwnerAdd homeOwnerAdd)
        {
            try
            {
                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");

                var existingHome = ktopdbContext.Homes
                    .FirstOrDefault(h => h.HomeName == homeOwnerAdd.HomeName && h.OwnerId == userId);
                if (existingHome != null) return Conflict("Home with the same name already exists for the owner");

                Home newHome = new()
                {
                    HomeName = homeOwnerAdd.HomeName,
                    OwnerId = userId
                };
                ktopdbContext.Homes.Add(newHome);
                ktopdbContext.SaveChanges();

                var existingUserHome = ktopdbContext.UsersHomes
                    .FirstOrDefault(uh => uh.UserId == userId && uh.HomeId == newHome.HomeId);
                if (existingUserHome != null) return Conflict("Relation between user and home already exists");

                UsersHome newUsersHome = new()
                {
                    UserId = userId,
                    HomeId = newHome.HomeId
                };
                ktopdbContext.UsersHomes.Add(newUsersHome);
                ktopdbContext.SaveChanges();

                return Ok("Home and UsersHome added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{userId}/{homeId}")]
        [Authorize]
        public IActionResult EditUserHome(int userId, int homeId, [FromBody] HomeModel homeModel)
        {
            try
            {
                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");

                var editHome = ktopdbContext.Homes.Find(homeId);
                if (editHome == null) return NotFound("Home not found");

                if (editHome.OwnerId != userId)
                    return StatusCode(403, "Only home owner can edit home");

                var owner = ktopdbContext.Users.Find(homeModel.OwnerId);
                if (owner == null) return NotFound("Owner not found");

                var isUserAddedToHome = ktopdbContext.UsersHomes
                    .Any(uh => uh.HomeId == homeId && uh.UserId == homeModel.OwnerId);
                if (!isUserAddedToHome)
                    return BadRequest("The new owner must be a user added to this home");

                var existingHome = ktopdbContext.Homes
                    .FirstOrDefault(h => h.HomeName == homeModel.HomeName && h.OwnerId == homeModel.OwnerId && h.HomeId != homeId);
                if (existingHome != null) return Conflict("Home with the same name already exists for the owner");

                editHome.HomeName = homeModel.HomeName;
                editHome.OwnerId = homeModel.OwnerId;

                ktopdbContext.SaveChanges();
                return Ok("Home updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{userId}/{homeId}")]
        [Authorize]
        public IActionResult DeleteUserHome(int userId, int homeId)
        {
            try
            {
                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");

                var deleteHome = ktopdbContext.Homes.Find(homeId);
                if (deleteHome == null) return NotFound("Home not found");

                if (deleteHome.OwnerId != userId)
                    return StatusCode(403, "Only the home owner can delete the home");

                var relatedProducts = ktopdbContext.Products
                    .Where(p => p.HomeId == homeId).ToList();
                ktopdbContext.Products.RemoveRange(relatedProducts);

                var relatedUserHomes = ktopdbContext.UsersHomes
                    .Where(uh => uh.HomeId == homeId).ToList();
                ktopdbContext.UsersHomes.RemoveRange(relatedUserHomes);

                ktopdbContext.Homes.Remove(deleteHome);
                ktopdbContext.SaveChanges();
                return Ok("Home and related records deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{homeId}")]
        [Authorize]
        public IActionResult GetAllHomeProducts(int homeId)
        {
            try
            {
                var home = ktopdbContext.Homes.Find(homeId);
                if (home == null) return NotFound("Home not found");

                var homeProducts = ktopdbContext.Products
                    .Where(p => p.HomeId == homeId);

                return Ok(homeProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{homeId}")]
        [Authorize]
        public IActionResult GetAllHomeUsers(int homeId)
        {
            try
            {
                var home = ktopdbContext.Homes.Find(homeId);
                if (home == null) return NotFound("Home not found");

                var homeUsers = ktopdbContext.UsersHomes
                    .Where(uh => uh.HomeId == homeId).Include(uh => uh.User);

                return Ok(homeUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{homeId}")]
        [Authorize]
        public IActionResult AddHomeUser(int homeId, [FromBody] string userEmail)
        {
            try
            {
                var home = ktopdbContext.Homes.Find(homeId);
                if (home == null) return NotFound("Home not found");

                var userExists = ktopdbContext.Users.FirstOrDefault(u => u.Email == userEmail);
                if (userExists == null) return NotFound("User not found");

                var usersHomeExists = ktopdbContext.UsersHomes
                    .FirstOrDefault(uh => uh.HomeId == homeId && uh.UserId == userExists.UserId);
                if (usersHomeExists != null) return Conflict("User is already added to the home");

                var newUserHome = new UsersHome
                {
                    HomeId = homeId,
                    UserId = userExists.UserId
                };

                ktopdbContext.UsersHomes.Add(newUserHome);
                ktopdbContext.SaveChanges();

                return Ok("User added to home successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{homeId}/{userId}/{loggedInUserId}")]
        [Authorize]
        public IActionResult DeleteHomeUser(int homeId, int userId, int loggedInUserId)
        {
            try
            {
                var home = ktopdbContext.Homes.Find(homeId);
                if (home == null) return NotFound("Home not found");

                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");

                var loggedInUser = ktopdbContext.Users.Find(loggedInUserId);
                if (loggedInUser == null) return NotFound("User not found");

                if (home.OwnerId != loggedInUserId)
                    return StatusCode(403, "Only the home owner can delete users from the home");

                if (home.OwnerId == loggedInUserId && loggedInUserId == userId)
                    return StatusCode(403, "Users cannot delete themselves from the home");

                var userHome = ktopdbContext.UsersHomes.FirstOrDefault(uh => uh.HomeId == homeId && uh.UserId == userId);
                if (userHome == null) return NotFound("User not found in the home");

                ktopdbContext.UsersHomes.Remove(userHome);
                ktopdbContext.SaveChanges();

                return Ok("User removed from home successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
