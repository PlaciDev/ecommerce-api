using EcommerceApi.Data;
using EcommerceApi.Models;
using EcommerceApi.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EcommerceApi.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet("api/products")]
        public async Task<IActionResult> Get(
            [FromServices] ApiDbContext context,
            [FromServices] IMemoryCache cache,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25)
        {
            try
            {
                var products = await cache.GetOrCreateAsync("products", async entry =>

                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

                    return await context
                    .Products
                    .AsNoTracking()
                    .Select(x => new ProductListViewModel

                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Price = x.Price,
                        Stock = x.Stock,
                        Category = new CategoryListViewModel
                        {
                            Id = x.Category.Id,
                            Name = x.Category.Name,
                            Description = x.Category.Description
                        }
                    })
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToListAsync();


                });




                    
                if (products is null || !products.Any())
                {
                    return NotFound("Nenhum produto encontrado...");
                }
                return Ok(products);
            }
            catch
            {
                return StatusCode(500, "Erro interno no servidor...");
            }
        }

        [HttpGet("api/products/{id:int}")]
        public async Task<IActionResult> GetById(
            [FromServices] ApiDbContext context,
            [FromRoute] int id,
            [FromServices] IMemoryCache cache)
        {
            try
            {
                var product = await cache.GetOrCreateAsync($"product_{id}", async entry =>

                {

                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

                    return await context
                   .Products
                   .AsNoTracking()
                   .Select(x => new ProductListViewModel
                   {
                       Id = x.Id,
                       Name = x.Name,
                       Description = x.Description,
                       Price = x.Price,
                       Stock = x.Stock,
                       Category = new CategoryListViewModel
                       {
                           Id = x.Category.Id,
                           Name = x.Category.Name,
                           Description = x.Category.Description
                       }
                   })
                   .FirstOrDefaultAsync(x => x.Id == id);

                });

                   
                if (product is null)
                {
                    return NotFound("Produto não encontrado...");
                }
                return Ok(product);
            }
            catch
            {
                return StatusCode(500, "Erro interno no servidor...");
            }


        }

        [HttpPost("api/products")]
        public async Task<IActionResult> Post(
            [FromServices] ApiDbContext context,
            [FromBody] ProductEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Name == model.CategoryName);

                if (category is null)
                {
                    return NotFound("Categoria não encontrada...");
                }

                var product = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    Stock = model.Stock,
                    Category = category

                };

                context.Products.Add(product);
                await context.SaveChangesAsync();

                var productViewModel = new ProductListViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock,
                    Category = new CategoryListViewModel
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Description = category.Description
                    }
                };

                return Created($"api/products/{product.Id}", productViewModel);
            }
            catch (Exception)
            {

                return StatusCode(500, "Erro interno no servidor...");
            }
        }

        [HttpPut("api/products/{id:int}")]
        public async Task<IActionResult> Put(
            [FromServices] ApiDbContext context,
            [FromBody] ProductEditViewModel model,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);

                if (product is null)
                {
                    return NotFound("Produto não encontrado...");
                }

                var category = await context.Categories.FirstOrDefaultAsync(x => x.Name == model.CategoryName);
                if (category is null)
                {
                    return NotFound("Categoria não encontrada...");
                }

                product.Name = model.Name;
                product.Description = model.Description;
                product.Price = model.Price;
                product.Stock = model.Stock;
                product.Category = category;

                context.Products.Update(product);
                await context.SaveChangesAsync();

                var productViewModel = new ProductListViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock,
                    Category = new CategoryListViewModel
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Description = category.Description
                    }
                };

                return Ok(productViewModel);


            }
            catch (Exception)
            {

                return StatusCode(500, "Erro interno no servidor...");
            }


        }

        [HttpDelete("api/products/{id:int}")]
        public async Task<IActionResult> DeleteById(
            [FromServices] ApiDbContext context,
            [FromRoute] int id)
        {
            try
            {
                var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
                if (product is null)
                {
                    return NotFound("Produto não encontrado...");
                }

                context.Products.Remove(product);
                await context.SaveChangesAsync();

                return Ok("Produto removido com sucesso...");
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno no servidor...");
            }
        }


    }
}
