using EcommerceApi.Data;
using EcommerceApi.Models;
using EcommerceApi.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Controllers

{
    [ApiController]
    public class RoleController : ControllerBase
    {

        [HttpGet("api/roles")]
        public async Task<IActionResult> Get(
            [FromServices] ApiDbContext context)
        {
            try
            {
                var roles = await context
                    .Roles
                    .AsNoTracking()
                    .Select(x => new RoleListViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description
                    })
                    .ToListAsync();
                if (roles is null || !roles.Any())
                {
                    return NotFound("Nenhum perfil encontrado...");
                }
                return Ok(roles);
            }
            catch
            {
                return StatusCode(500, "Erro interno no servidor...");
            }
        }

        [HttpGet("api/roles/{id:int}")]
        public async Task<IActionResult> GetById(
            [FromServices] ApiDbContext context,
            [FromRoute] int id)
        {
            try
            {
                var role = await context
                    .Roles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (role is null)
                {
                    return NotFound("Perfil não encontrado...");
                }
                return Ok(role);
            }
            catch
            {
                return StatusCode(500, "Erro interno no servidor...");
            }
        }

        [HttpPost("api/roles")]
        public async Task<IActionResult> Post(
            [FromServices] ApiDbContext context,
            [FromBody] RoleEditViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var role = new Role
                {
                    Name = model.Name,
                    Description = model.Description
                };

                await context.Roles.AddAsync(role);
                await context.SaveChangesAsync();

                var roleViewModel = new RoleListViewModel
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description
                };

                return Created($"api/roles/{role.Id}", roleViewModel);
            }
            catch
            {
                return StatusCode(500, "Erro interno no servidor...");

            }
        }

        [HttpPut("api/roles/{id:int}")]
        public async Task<IActionResult> Put(
            [FromServices] ApiDbContext context,
            [FromRoute] int id,
            [FromBody] Role model)
        {
            try
            {
                var role = await context.Roles.FirstOrDefaultAsync(x => x.Id == id);
                if (role is null)
                {
                    return NotFound("Perfil não encontrado...");
                }

                role.Name = model.Name;
                role.Description = model.Description;

                context.Roles.Update(role);
                await context.SaveChangesAsync();

                return Ok(role);
            }
            catch
            {
                return StatusCode(500, "Erro interno no servidor...");
            }
        }

        [HttpDelete("api/roles/{id:int}")]
        public async Task<IActionResult> Delete(
            [FromServices] ApiDbContext context,
            [FromRoute] int id)
        {

            try
            {
                var role = await context.Roles.FirstOrDefaultAsync(x => x.Id == id);
                if (role is null)
                {
                    return NotFound("Perfil não encontrado...");
                }

                context.Roles.Remove(role);
                await context.SaveChangesAsync();

                return Ok("Perfil removido com sucesso...");
            }
            catch
            {
                return StatusCode(500, "Erro interno no servidor...");
            }
        }


    }
}
