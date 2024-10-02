using KTOPWebAPI.Entities;
using KTOPWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace KTOPWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly KtopdbContext ktopdbContext;
        private readonly IConfiguration config;

        public UserController(KtopdbContext ktopdbContext, IConfiguration config)
        {
            this.ktopdbContext = ktopdbContext;
            this.config = config;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllUsers()
        {
            try
            {
                return Ok(ktopdbContext.Users.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        [Authorize]
        public IActionResult GetUserById(int userId)
        {
            try
            {
                var user = ktopdbContext.Users.Find(userId);
                if (user == null) return NotFound("User not found");
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userName}")]
        [Authorize]
        public IActionResult SearchUserByName(string userName)
        {
            try
            {
                var searchUserName = ktopdbContext.Users.Where(u => u.UserName.Contains(userName));
                if (searchUserName == null) return NotFound("User not found");
                return Ok(searchUserName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{email}")]
        [Authorize]
        public IActionResult SearchUserByEmail(string email)
        {
            try
            {
                var searchUserEmail = ktopdbContext.Users.Where(u => u.Email.Contains(email));
                if (searchUserEmail == null) return NotFound("User not found");
                return Ok(searchUserEmail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Register([FromBody] Register register)
        {
            try
            {
                var userExists = ktopdbContext.Users.FirstOrDefault(u => u.Email == register.Email);
                if (userExists != null) return BadRequest("User with the same email already exists");

                string salt = GenerateUniqueSalt();
                string hashedPassword = HashPassword(register.Password, salt);

                User newUser = new()
                {
                    UserName = register.UserName,
                    Email = register.Email,
                    Password = hashedPassword,
                    Salt = salt,
                    LastModified = DateTime.Now
                };
                ktopdbContext.Users.Add(newUser);
                ktopdbContext.SaveChanges();

                var existingHome = ktopdbContext.Homes
                    .FirstOrDefault(h => h.HomeName == "My Home" && h.OwnerId == newUser.UserId);
                if (existingHome != null) return Conflict("A home named 'My Home' already exists for the owner");

                Home newHome = new()
                {
                    HomeName = "My Home",
                    OwnerId = newUser.UserId
                };
                ktopdbContext.Homes.Add(newHome);
                ktopdbContext.SaveChanges();

                var existingUserHome = ktopdbContext.UsersHomes
                    .FirstOrDefault(uh => uh.UserId == newUser.UserId && uh.HomeId == newHome.HomeId);
                if (existingUserHome != null) return Conflict("Relation between user and home already exists");

                UsersHome newUsersHome = new()
                {
                    UserId = newUser.UserId,
                    HomeId = newHome.HomeId
                };
                ktopdbContext.UsersHomes.Add(newUsersHome);
                ktopdbContext.SaveChanges();

                return Ok("User registered successfully with home");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost]
        public IActionResult Login([FromBody] Login login)
        {
            try
            {
                var user = ktopdbContext.Users.SingleOrDefault(u => u.Email == login.Email);
                if (user == null) return NotFound("Incorrect email");

                string hashedPassword = HashPassword(login.Password, user.Salt);
                if (hashedPassword != user.Password) return Unauthorized("Incorrect email or password");

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                    new Claim(ClaimTypes.Email , user.Email)
                };

                var token = new JwtSecurityToken(
                    issuer: config["JWT:Issuer"],
                    audience: config["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(60),
                    signingCredentials: credentials);
                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return new ObjectResult(new
                {
                    access_token = jwt,
                    token_type = "bearer",
                    user_id = user.UserId,
                    user_name = user.UserName
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{userId}")]
        [Authorize]
        public IActionResult EditUserData(int userId, [FromBody] UserEditData userEditData)
        {
            try
            {
                var editUser = ktopdbContext.Users.Find(userId);
                if (editUser == null) return NotFound("User not found");

                var existingUserEmail = ktopdbContext.Users
                    .FirstOrDefault(u => u.Email == userEditData.Email && u.UserId != userId);
                if (existingUserEmail != null) return BadRequest("Email is already in use");

                editUser.UserName = userEditData.UserName;
                editUser.Email = userEditData.Email;
                editUser.LastModified = DateTime.Now;

                ktopdbContext.SaveChanges();
                return Ok("User data updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{userId}")]
        [Authorize]
        public IActionResult ChangePassword(int userId, [FromBody] UserChangePwd userChangePwd)
        {
            try
            {
                var editUser = ktopdbContext.Users.Find(userId);
                if (editUser == null) return NotFound("User not found");

                string currentPasswordHash = HashPassword(userChangePwd.CurrentPassword, editUser.Salt);
                if (currentPasswordHash != editUser.Password) return BadRequest("Incorrect current password");

                if (userChangePwd.NewPassword != userChangePwd.ConfirmPassword)
                    return BadRequest("New password and confirm password do not match");

                string newSalt = GenerateUniqueSalt();
                string newPasswordHash = HashPassword(userChangePwd.NewPassword, newSalt);

                editUser.Password = newPasswordHash;
                editUser.Salt = newSalt;
                editUser.LastModified = DateTime.Now;

                ktopdbContext.SaveChanges();
                return Ok("Password changed successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{userId}")]
        [Authorize]
        public IActionResult DeleteUser(int userId, [FromBody] UserDeleteAccount userDeleteAccount)
        {
            try
            {
                var deleteUser = ktopdbContext.Users.Find(userId);
                if (deleteUser == null) return NotFound("User not found");

                string hashedPassword = HashPassword(userDeleteAccount.Password, deleteUser.Salt);
                if (hashedPassword != deleteUser.Password) return BadRequest("Incorrect password");

                var userHomes = ktopdbContext.Homes.Where(h => h.OwnerId == userId).ToList();
                foreach (var home in userHomes)
                {
                    var homeProducts = ktopdbContext.Products.Where(p => p.HomeId == home.HomeId).ToList();
                    ktopdbContext.Products.RemoveRange(homeProducts);

                    var homeUsers = ktopdbContext.UsersHomes.Where(uh => uh.HomeId == home.HomeId).ToList();
                    ktopdbContext.UsersHomes.RemoveRange(homeUsers);

                    ktopdbContext.Homes.Remove(home);
                }

                var userHomeRelations = ktopdbContext.UsersHomes.Where(uh => uh.UserId == userId).ToList();
                ktopdbContext.UsersHomes.RemoveRange(userHomeRelations);

                ktopdbContext.Users.Remove(deleteUser);
                ktopdbContext.SaveChanges();
                return Ok("User and associated data deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private string GenerateUniqueSalt()
        {
            byte[] saltBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);
            if (ktopdbContext.Users.Any(u => u.Salt == salt)) return GenerateUniqueSalt();
            return salt;
        }

        private static string HashPassword(string password, string salt)
        {
            byte[] combinedBytes = Encoding.UTF8.GetBytes(password + salt);
            byte[] hashedBytes = SHA256.HashData(combinedBytes);
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
