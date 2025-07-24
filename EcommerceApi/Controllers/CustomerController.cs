using EcommerceApi.Data;
using EcommerceApi.Models;
using EcommerceApi.Services;
using EcommerceApi.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace EcommerceApi.Controllers
{
    [ApiController]
    public class CustomerController : ControllerBase
    {


        [HttpPost("api/customers/login")]
        public async Task<IActionResult> Login(
            [FromServices] ApiDbContext context,
            [FromServices] TokenService tokenService,
            [FromBody] CustomerLoginViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var customer = await context
                .Customers
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Email == model.Email);

            if (customer is null)
            {
                return NotFound("Cliente não encontrado...");
            }

            if (!PasswordHasher.Verify(customer.User.PasswordHash, model.Password))
            {
                return NotFound("Senha inválida...");
            }

            try
            {
                var token = tokenService.GenerateToken(customer.User);
                return Ok(token);
            }
            catch (Exception)
            {

                return StatusCode(500, "Erro interno no servidor...");
            }
        }

        [HttpPost("api/customers/register")]
        public async Task<IActionResult> Register(
            [FromServices] ApiDbContext context,
            [FromBody] CustomerRegisteViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var role = context.Roles.FirstOrDefaultAsync(x => x.Name == "Customer");
                var customer = new Customer
                {
                    User = new User
                    {
                        Name = model.User.Name,
                        Email = model.User.Email,
                        PasswordHash = PasswordHasher.Hash(model.User.Password),
                        Role = await role
                    },

                    PhoneNumber = model.PhoneNumber,

                    Addresses = new List<Address>
                    {
                        new Address
                        {
                            Street = model.Address.Street,
                            City = model.Address.City,
                            State = model.Address.State,
                            ZipCode = model.Address.ZipCode
                        }
                    }
                };

                if (customer is null)
                {
                    return BadRequest("Erro ao criar cliente...");
                }

                if (await context.Customers.AnyAsync(x => x.User.Email == customer.User.Email))
                {
                    return BadRequest("Email já cadastrado...");
                }

                context.Customers.Add(customer);
                context.SaveChanges();

                return Created($"api/customers/{customer.Id}", customer);



            }
            catch (Exception)
            {

                throw;
            }
        }

















    }

}
