using EcommerceApi.Data;
using EcommerceApi.Models;
using EcommerceApi.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EcommerceApi.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {

        [HttpGet("api/categories")]
        public async Task<IActionResult> Get(
            [FromServices] ApiDbContext context,
            [FromServices] IMemoryCache cache,
            [FromQuery] int page,
            [FromQuery] int pageSize)
        {
            try
            {
                var categories = await cache.GetOrCreateAsync("categories", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);


                    return await context
                        .Categories
                        .AsNoTracking()
                        .Select(x => new CategoryListViewModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Description = x.Description
                        })
                        .Skip(page * pageSize)
                        .Take(pageSize)
                        .OrderBy(x => x.Name)
                        .ToListAsync();

                });
                

                if (categories is null || !categories.Any())
                {
                    return NotFound("Nenhuma categoria encontrada...");
                }

                return Ok(categories);
            }
            catch
            {
                return StatusCode(500, "Erro interno no servidor...");

            }
        }

        [HttpGet("api/categories/with-products")]
        public async Task<IActionResult> GetWithProducts(
            [FromServices] ApiDbContext context,
            [FromServices] IMemoryCache cache,
            [FromQuery] int size,
            [FromQuery] int pageSize)
        {
            try
            {
                var categories = await cache.GetOrCreateAsync("categories_with_products", async entry =>
                {

                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

                    return await context
                   .Categories
                   .AsNoTracking()
                   .Include(x => x.Products)
                   .Select(x => new CategoryListViewModel
                   {
                       Id = x.Id,
                       Name = x.Name,
                       Description = x.Description,
                       Products = x.Products.Select(x => new ProductListViewModel
                       {
                           Id = x.Id,
                           Name = x.Name,
                           Price = x.Price,
                           Description = x.Description
                       }).ToList()
                   })
                   .Skip(size * pageSize)
                   .Take(pageSize)
                   .OrderBy(x => x.Name)
                   .ToListAsync();

                });
                   
                if (categories is null || !categories.Any())
                {
                    return NotFound("Nenhuma categoria encontrada...");
                }
                return Ok(categories);
            }
            catch
            {
                return StatusCode(500, "Erro interno no servidor...");
            }
        }

        [HttpGet("api/categories/{id:int}")]
        public async Task<IActionResult> GetById(
            [FromServices] ApiDbContext context,
            [FromServices] IMemoryCache cache,
            [FromRoute] int id)
        {
            try
            {
                var category = cache.GetOrCreateAsync<CategoryListViewModel>($"category_{id}", async entry =>

                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);


                    return await context
                    .Categories
                    .AsNoTracking()
                    .Select(x => new CategoryListViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Products = x.Products.Select(x => new ProductListViewModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Price = x.Price,
                            Description = x.Description
                        }).ToList()
                    })
                    .FirstOrDefaultAsync(x => x.Id == id);
                });

                    
                if (category is null)
                {
                    return NotFound("Categoria não encontrada...");
                }
                return Ok(category);
            }
            catch
            {
                return StatusCode(500, "Erro interno no servidor...");
            }
        }

        

        [HttpPost("api/categories")]
        public async Task<IActionResult> Post(
            [FromServices] ApiDbContext context,
            [FromBody] CategoryEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var category = new Category
                {
                    Name = model.Name,
                    Description = model.Description
                };

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                var categoryViewModel = new CategoryListViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                };

                return Created($"api/categories/{category.Id}", categoryViewModel);
            }
            catch
            {
                return StatusCode(500, "Erro interno no servidor...");

            }
        }

        [HttpPut("api/categories/{id:int}")]
        public async Task<IActionResult> Put(
            [FromServices] ApiDbContext context,
            [FromRoute] int id,
            [FromBody] CategoryEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category is null)
                {
                    return NotFound("Categoria não encontrada...");
                }

                category.Name = model.Name;
                category.Description = model.Description;

                context.Update(category);
                await context.SaveChangesAsync();

                return Ok(category);
            }
            catch (Exception)
            {

                return StatusCode(500, "Erro interno no servidor...");
            }
        }

        [HttpDelete("api/categories/{id:int}")]
        public async Task<IActionResult> Delete(
            [FromServices] ApiDbContext context,
            [FromRoute] int id)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category is null)
                {
                    return NotFound("Categoria não encontrada...");
                }

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return Ok("Categoria removida com sucesso...");
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno no servidor...");
            }


        }



    }
}
