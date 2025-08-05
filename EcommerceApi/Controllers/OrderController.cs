using EcommerceApi.Data;
using EcommerceApi.Models;
using EcommerceApi.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EcommerceApi.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpGet("api/orders")]
        public async Task<IActionResult> Get(
            [FromServices] ApiDbContext context,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25)
        {
            try
            {
                var orders = await context
                    .Orders
                    .AsNoTracking()
                    .Include(x => x.Customer)
                    .Include(x => x.Payment)
                    .Include(x => x.OrderItems)
                        .ThenInclude(x => x.Product)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (orders == null || !orders.Any())
                {
                    return NotFound("Nenhum pedido encontrado...");
                }

                return Ok(orders);
            }
            catch
            {

                return StatusCode(500, "Erro interno no servidor...");
            }
        }

        [HttpGet("api/orders/{id:int}")]
        public async Task<IActionResult> GetById(
            [FromServices] ApiDbContext context,
            int id)
        {
            try
            {
                var order = await context
                    .Orders
                    .AsNoTracking()
                    .Include(x => x.Customer)
                    .Include(x => x.Customer.User)
                    .Include(x => x.Customer.User.Role)
                    .Include(x => x.Payment)
                    .Include(x => x.OrderItems)
                    
                        .ThenInclude(x => x.Product)
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (order == null)
                {
                    return NotFound("Pedido não encontrado...");
                }
                return Ok(order);
            }
            catch
            {
                return StatusCode(500, "Erro interno no servidor...");
            }
        }
        
        [HttpPost("api/orders")]
        public async Task<IActionResult> Post(
            [FromServices] ApiDbContext context,
            [FromBody] OrderCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            try
            {
                if (model.PaymentMethod == null)
                    return BadRequest("Método de pagamento não informado...");

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);


                var customer = await context
                    .Customers
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.User.Id == userId);

                if (customer == null)
                    return NotFound("Cliente não encontrado...");

                var orderItems = new List<OrderItem>();
                decimal total = 0;

                foreach (var item in model.OrderItems)
                {
                    var product = await context.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                    if (product == null)
                        return NotFound($"Produto com ID {item.ProductId} não encontrado...");

                    if (product.Stock < item.Quantity)
                        return BadRequest($"Estoque insuficiente para o produto {product.Name}.");

                    product.Stock -= item.Quantity;

                    OrderItem orderItem = new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price
                    };

                    total += product.Price * item.Quantity;
                    orderItems.Add(orderItem);
           
                }

                var payment = new Payment
                {
                    Method = model.PaymentMethod,
                    Amount = total,
                    Date = DateTime.UtcNow,
                    Status = PaymentStatus.Pending
                };

                var order = new Order
                {
                    Date = DateTime.UtcNow,
                    TotalAmount = total,
                    Status = Order.OrderStatus.Pending,
                    Customer = customer,
                    Payment = payment,
                    OrderItems = orderItems
                };

                context.Orders.Add(order);
                await context.SaveChangesAsync();

                var orderViewModel = new OrderListViewModel
                {
                    Id = order.Id,
                    Date = order.Date,
                    TotalAmount = order.TotalAmount,
                    Status = (OrderListViewModel.OrderStatus)order.Status,
                    CustomerId = customer.Id,
                    Customer = customer,
                    Payment = payment,
                    OrderItems = order.OrderItems
                };

                return Created($"api/orders/{order.Id}", orderViewModel);
            }
            catch
            {
                return StatusCode(500, "Erro interno no servidor...");

            }

            
        }

        [HttpPost("api/orders/{id:int}/cancel")]
        public async Task<IActionResult> Cancel(
            [FromServices] ApiDbContext context,
            [FromRoute] int id)
            {
            try
            {
                var order = await context.Orders
                    .Include(x => x.OrderItems)
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (order == null)
                {
                    return NotFound("Pedido não encontrado...");
                }
                if (order.Status == Order.OrderStatus.Delivered || order.Status == Order.OrderStatus.Cancelled)
                {
                    return BadRequest("Não é possível cancelar um pedido já entregue ou cancelado.");
                }
                order.Status = Order.OrderStatus.Cancelled;
                foreach (var item in order.OrderItems)
                {
                    var product = await context.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                    if (product != null)
                    {
                        product.Stock += item.Quantity;
                    }
                }
                await context.SaveChangesAsync();
                return Ok("Pedido cancelado com sucesso...");
            }
            catch
            {
                return StatusCode(500, "Erro interno no servidor...");
            }
        }
    }
}
