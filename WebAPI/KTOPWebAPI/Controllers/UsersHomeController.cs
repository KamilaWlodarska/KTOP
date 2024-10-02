using KTOPWebAPI.Entities;
using KTOPWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KTOPWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersHomeController : ControllerBase
    {
        private readonly KtopdbContext ktopdbContext;

        public UsersHomeController(KtopdbContext ktopdbContext)
        {
            this.ktopdbContext = ktopdbContext;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllUsersHomes()
        {
            try
            {
                return Ok(ktopdbContext.UsersHomes.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{usersHomeId}")]
        [Authorize]
        public IActionResult GetUsersHomeById(int usersHomeId)
        {
            try
            {
                var usersHome = ktopdbContext.UsersHomes.Find(usersHomeId);
                if (usersHome == null) return NotFound("Users Home not found");
                return Ok(usersHome);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddUsersHome([FromBody] UsersHomeModel usersHomeModel)
        {
            try
            {
                var user = ktopdbContext.Users.Find(usersHomeModel.UserId);
                if (user == null) return NotFound("User not found");

                var home = ktopdbContext.Homes.Find(usersHomeModel.HomeId);
                if (home == null) return NotFound("Home not found");

                var existingUsersHome = ktopdbContext.UsersHomes
                    .FirstOrDefault(uh => uh.UserId == usersHomeModel.UserId && uh.HomeId == usersHomeModel.HomeId);
                if (existingUsersHome != null) return Conflict("Users Home already exists");

                UsersHome newUsersHome = new()
                {
                    UserId = usersHomeModel.UserId,
                    HomeId = usersHomeModel.HomeId
                };
                ktopdbContext.UsersHomes.Add(newUsersHome);
                ktopdbContext.SaveChanges();
                return Ok("Users Home added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{usersHomeId}")]
        [Authorize]
        public IActionResult EditUsersHome(int usersHomeId, [FromBody] UsersHomeModel usersHomeModel)
        {
            try
            {
                var editUsersHome = ktopdbContext.UsersHomes.Find(usersHomeId);
                if (editUsersHome == null) return NotFound("Users Home not found");

                var user = ktopdbContext.Users.Find(usersHomeModel.UserId);
                if (user == null) return NotFound("User not found");

                var home = ktopdbContext.Homes.Find(usersHomeModel.HomeId);
                if (home == null) return NotFound("Home not found");

                var existingUsersHome = ktopdbContext.UsersHomes
                    .FirstOrDefault(uh => uh.UserId == usersHomeModel.UserId && uh.HomeId == usersHomeModel.HomeId && uh.UsersHomesId != usersHomeId);
                if (existingUsersHome != null) return Conflict("Users Home already exists");

                editUsersHome.UserId = usersHomeModel.UserId;
                editUsersHome.HomeId = usersHomeModel.HomeId;

                ktopdbContext.SaveChanges();
                return Ok("Users Home updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{usersHomeId}")]
        [Authorize]
        public IActionResult DeleteUsersHome(int usersHomeId)
        {
            try
            {
                var deleteUsersHome = ktopdbContext.UsersHomes.Find(usersHomeId);
                if (deleteUsersHome == null) return NotFound("Users Home not found");

                var user = ktopdbContext.Users.FirstOrDefault(u => u.UserId == deleteUsersHome.UserId);
                if (user == null) return NotFound("User not found");

                var home = ktopdbContext.Homes.FirstOrDefault(h => h.HomeId == deleteUsersHome.HomeId);
                if (home == null) return NotFound("Home not found");

                if (user.UserId == home.OwnerId)
                    return BadRequest("Cannot remove association. User is the owner of the home.");

                ktopdbContext.UsersHomes.Remove(deleteUsersHome);
                ktopdbContext.SaveChanges();
                return Ok("Users Home deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
