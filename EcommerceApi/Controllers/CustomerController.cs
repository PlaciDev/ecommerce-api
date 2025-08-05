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
                .Include(x => x.User.Role)
                .FirstOrDefaultAsync(x => x.User.Email == model.Email);

            if (customer is null)
            {
                return NotFound("Usuário ou senha inválida...");
            }

            if (!PasswordHasher.Verify(customer.User.PasswordHash, model.Password))
            {
                return NotFound("Usuário ou senha inválida...");
            }

            try
            {
                var token = tokenService.GenerateToken(customer.User);
                return Ok(token);
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
                
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

                var customerViewModel = new CustomerListViewModel
                {
                    Id = customer.Id,
                    PhoneNumber = customer.PhoneNumber,
                    User = new UserListViewModel
                    {
                        Id = customer.User.Id,
                        Name = customer.User.Name,
                        Email = customer.User.Email,
                        CreatedAt = customer.User.CreatedAt,
                        Role = new RoleListViewModel
                        {
                            Id = customer.User.Role.Id,
                            Name = customer.User.Role.Name,
                            Description = customer.User.Role.Description
                        }
                    },
                    Addresses = customer.Addresses.Select(x => new AddressListViewModel
                    {
                        Id = x.Id,
                        Street = x.Street,
                        City = x.City,
                        State = x.State,
                        ZipCode = x.ZipCode
                    }).ToList()
                };

                return Created($"api/customers/{customer.Id}", customerViewModel);



            }
            catch (Exception)
            {

                return StatusCode(500, "Erro interno no servidor...");
            }
        }

        [HttpGet("api/customers")]
        public async Task<IActionResult> Get(
            [FromServices] ApiDbContext context,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25)
        {
            try
            {
                var customers = context
                    .Customers
                    .AsNoTracking()
                    .Include(x => x.User)
                        .ThenInclude(x => x.Role)
                    .Include(x => x.Addresses)
                    .Select(x => new CustomerListViewModel
                    {
                        Id = x.Id,
                        PhoneNumber = x.PhoneNumber,
                        User = new UserListViewModel
                        {
                            Id = x.User.Id,
                            Name = x.User.Name,
                            Email = x.User.Email,
                            CreatedAt = x.User.CreatedAt,
                            Role = new RoleListViewModel
                            {
                                Id = x.User.Role.Id,
                                Name = x.User.Role.Name,
                                Description = x.User.Role.Description
                            }
                        },

                        Addresses = x.Addresses.Select(x => new AddressListViewModel
                        {
                            Id = x.Id,
                            Street = x.Street,
                            City = x.City,
                            State = x.State,
                            ZipCode = x.ZipCode
                        })
                        .Skip(page * pageSize)
                        .Take(pageSize)
                        .ToList()


                    });
                   

                if (customers is null || !customers.Any())
                {
                    return NotFound("Nenhum cliente encontrado...");
                }

                return Ok(customers);


            }
            catch (Exception)
            {

                return StatusCode(500, "Erro interno no servidor...");
            }
        }

        [HttpGet("api/customers/{id:int}")]
        public async Task<IActionResult> GetById(
            [FromServices] ApiDbContext context,
            [FromRoute] int id)
        {
            try
            {
                var customer = await context
                    .Customers
                    .AsNoTracking()
                    .Include(x => x.User)
                        .ThenInclude(x => x.Role)
                    .Include(x => x.Addresses)
                    .Include(x => x.Orders)
                    .Select(x => new CustomerListViewModel
                    {
                        Id = x.Id,
                        PhoneNumber = x.PhoneNumber,
                        User = new UserListViewModel
                        {
                            Id = x.User.Id,
                            Name = x.User.Name,
                            Email = x.User.Email,
                            CreatedAt = x.User.CreatedAt,
                            Role = new RoleListViewModel
                            {
                                Id = x.User.Role.Id,
                                Name = x.User.Role.Name,
                                Description = x.User.Role.Description
                            }
                        },
                        Addresses = x.Addresses.Select(x => new AddressListViewModel
                        {
                            Id = x.Id,
                            Street = x.Street,
                            City = x.City,
                            State = x.State,
                            ZipCode = x.ZipCode
                        }),

                        Orders = x.Orders.Select(x => new OrderListViewModel
                        {
                            Id = x.Id,
                            Date = x.Date,
                            TotalAmount = x.TotalAmount,
                            Status = (OrderListViewModel.OrderStatus)x.Status
                        }).ToList()

                    })
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (customer is null)
                {
                    return NotFound("Cliente não encontrado...");
                }
                return Ok(customer);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno no servidor...");
            }


        }

        [HttpPut("api/customers/{id:int}")]
        public async Task<IActionResult> Update(
            [FromServices] ApiDbContext context,
            [FromRoute] int id,
            [FromBody] CustomerEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var customer = await context
                    .Customers
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (customer is null)
                {
                    return NotFound("Cliente não encontrado...");
                }


                customer.User.Name = model.User.Name;
                customer.User.Email = model.User.Email;
                customer.PhoneNumber = model.PhoneNumber;
                customer.User.PasswordHash = PasswordHasher.Hash(model.User.Password);
                customer.Addresses = new List<Address>
                {
                    new Address
                    {
                        Street = model.Address.Street,
                        City = model.Address.City,
                        State = model.Address.State,
                        ZipCode = model.Address.ZipCode
                    }
                };

                context.Customers.Update(customer);
                await context.SaveChangesAsync();

                return Ok(customer);



            }
            catch (Exception)
            {

                return StatusCode(500, "Erro interno no servidor...");
            }
        }

        [HttpDelete("api/customers/{id:int}")]
        public async Task<IActionResult> Delete(
            [FromServices] ApiDbContext context,
            [FromRoute] int id)
        {
            try
            {
                var customer = await context
                    .Customers
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (customer is null)
                {
                    return NotFound("Cliente não encontrado...");
                }

                context.Customers.Remove(customer);
                await context.SaveChangesAsync();

                return Ok("Cliente removido com sucesso...");
            }
            catch
            {
                return StatusCode(500, "Erro interno no servidor...");
            }

        }
    }
}