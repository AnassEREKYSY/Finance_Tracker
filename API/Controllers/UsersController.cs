using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Dtos;
using Core.Entities;
using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService, IConfiguration configuration) : ControllerBase
    {
        
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await userService.RegisterUserAsync(registerDto);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(registerDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await userService.LoginUserAsync(loginDto);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    Console.WriteLine("Account is locked out.");
                }
                if (result.IsNotAllowed)
                {
                    Console.WriteLine("Account is not allowed.");
                }

                return Unauthorized("Invalid login attempt");
            }

            var user = await userService.GetUserByEmailAsync(loginDto.Email);

            var token = GenerateJwtToken(user!);

            return Ok(new { token });
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            if (!User.Identity.IsAuthenticated) return Unauthorized("User is not authenticated");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token");

            var user = await userService.GetUserProfileAsync(userId);

            if (user == null)
                return NotFound("User not found");

            return Ok(new
            {
                user.UserName,
                user.Email,
                user.FirstName,
                user.LastName
            });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout(){
            await userService.LogOutAsync();
            return Ok("");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getAll")]
        public async Task<ActionResult<List<AppUser>>> GetAllUsers()
        {
            var users = await userService.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("one/{id}")]
        public async Task<ActionResult<AppUser>> GetUserById(string id)
        {
            var user = await userService.GetUserByIdAsync(id);
            if (user == null) return NotFound("User not found");
            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateDto updateUserDto)
        {
            var updatedUser = new AppUser
            {
                FirstName = updateUserDto.FirstName,
                LastName = updateUserDto.LastName,
                Email = updateUserDto.Email,
                UserName = updateUserDto.UserName
            };

            var user = await userService.UpdateUserAsync(id, updatedUser);

            if (user == null)
                return BadRequest("Failed to update user");

            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await userService.DeleteUserAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }


        private string GenerateJwtToken(AppUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
