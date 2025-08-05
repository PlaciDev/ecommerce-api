using EcommerceApi.Data;
using EcommerceApi.Models;
using EcommerceApi.Services;
using EcommerceApi.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace EcommerceApi.Controllers
{   
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("api/users/register")]
        public async Task<IActionResult> Register(
            [FromServices] ApiDbContext context,
            [FromBody] UserRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var role = await context.Roles.FirstOrDefaultAsync(x => x.Name == "Default");
                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    PasswordHash = PasswordHasher.Hash(model.Password),
                    Role = role
                };

                if (user is null)
                {
                    return BadRequest("Dados inválidos...");
                }

                if (await context.Users.AnyAsync(x => x.Email == model.Email))
                {
                    return BadRequest("Já existe um usuário com este e-mail...");
                }

                context.Users.Add(user);
                await context.SaveChangesAsync();

                var UserListViewModel = new UserListViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                    Role = new RoleListViewModel
                    {
                        Id = user.Role.Id,
                        Name = user.Role.Name,
                        Description = user.Role.Description
                    }
                };

                return Created($"api/users/{user.Id}", user);
            }
            catch
            {
                return StatusCode(500, "Erro interno no servidor...");
          
            }

        }

        [AllowAnonymous]
        [HttpPost("api/users/login")]
        public async Task<IActionResult> Login(
            [FromServices] ApiDbContext context,
            [FromServices] TokenService tokenService,
            [FromBody] UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await context
                .Users
                .AsNoTracking()
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user is null)
            {
                return NotFound("Usuário ou senha inválidos...");
            }

            if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
            {
                return NotFound("Usuário ou senha inválidos...");
            }

            try
            {
                var token = tokenService.GenerateToken(user);
                return Ok(token);
            }
            catch
            {

                return StatusCode(500, "Erro interno no servidor...");
            }
        }







        
        [HttpGet("api/users")]
        public async Task<IActionResult> Get(
            [FromServices] ApiDbContext context,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25)
        {
            try
            {
                var users = await context
                    .Users
                    .AsNoTracking()
                    .Include(x => x.Role)
                    .Select(x => new UserListViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                        CreatedAt = x.CreatedAt,
                        Role = new RoleListViewModel
                        {
                            Id = x.Role.Id,
                            Name = x.Role.Name,
                            Description = x.Role.Description
                        }
                    })
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToListAsync();



                if (users is null || !users.Any())
                {
                    return NotFound("Nenhum usuário encontrado...");
                }
                return Ok(users);
            }
            catch
            {
                return StatusCode(500, "Erro interno no servidor...");
            }
        }

       
        [HttpGet("api/users/{id:int}")]
        public async Task<IActionResult> GetById(
            [FromServices] ApiDbContext context,
            [FromRoute] int id)
        {
            try
            {
                var user = await context
                    .Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (user is null)
                {
                    return NotFound("Usuário não encontrado...");
                }
                return Ok(user);
            }
            catch
            {
                return StatusCode(500, "Erro interno no servidor...");
            }
        }
        
        [HttpPost("api/users")]
        public async Task<IActionResult> Post(
            [FromServices] ApiDbContext context,
            [FromBody] UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var role = await context.Roles.FirstOrDefaultAsync(x => x.Name == model.RoleName);

                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    PasswordHash = PasswordHasher.Hash(model.Password),
                    Role = role
                };

                if (user is null)
                {
                    return BadRequest("Dados inválidos...");
                }

                if (await context.Users.AnyAsync(x => x.Email == model.Email))
                {
                    return BadRequest("Já existe um usuário com este e-mail...");
                }

                context.Users.Add(user);
                await context.SaveChangesAsync();

                var userViewModel = new UserListViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                    Role = new RoleListViewModel
                    {
                        Id = user.Role.Id,
                        Name = user.Role.Name,
                        Description = user.Role.Description
                    }
                };

                return Created($"api/users/{user.Id}", userViewModel);
            }
            catch
            {

                return StatusCode(500, "Erro interno no servidor...");

            }
        }

        [HttpPut("api/users/{id:int}")]
        public async Task<IActionResult> Put(
            [FromServices] ApiDbContext context,
            [FromRoute] int id,
            [FromBody] UserEditViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var role = await context.Roles.FirstOrDefaultAsync(x => x.Name == model.RoleName);

                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (user is null)
                {
                    return NotFound("Usuário não encontrado...");
                }
                user.Name = model.Name;
                user.Email = model.Email;
                user.PasswordHash = PasswordHasher.Hash(model.Password);
                user.Role = role;

                context.Users.Update(user);

                await context.SaveChangesAsync();

                var userViewModel = new UserListViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                    Role = new RoleListViewModel
                    {
                        Id = user.Role.Id,
                        Name = user.Role.Name,
                        Description = user.Role.Description
                    }
                };

                return Ok(userViewModel);
            }
            catch
            {
                return StatusCode(500, "Erro interno no servidor...");
            }
        }

        [HttpDelete("api/users/{id:int}")]
        public async Task<IActionResult> Delete(
            [FromServices] ApiDbContext context,
            [FromRoute] int id)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (user is null)
                {
                    return NotFound("Usuário não encontrado...");
                }

                context.Users.Remove(user);

                await context.SaveChangesAsync();
                return Ok("Usuário removido com sucesso...");
            }
            catch
            {
                return StatusCode(500, "Erro interno no servidor...");
            }
        }
    }
}
