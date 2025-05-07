using Microsoft.AspNetCore.Mvc;
using SuParty.Data;
using SuParty.Models;
using System;
using System.Diagnostics;
using Utility.Service.Product.enums;

namespace SuParty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public OrdersController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
        {
            var user = await _db.Users.FindAsync(request.UserId);
            if (user == null)
                return NotFound("User not found");

            var order = new Order
            {
                UserId = request.UserId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = request.Items.Sum(i => i.Quantity * i.UnitPrice),
                Status = OrderStatus.Pending,
                Payments = new List<Payment>
            {
                new Payment
                {
                    Amount = request.Payment.Amount,
                    Method = request.Payment.Method,
                    PaidAt = DateTime.UtcNow,
                    Status = PaymentStatus.Paid
                }
            }
            };

            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();

            return Ok(new { order.Id });
        }
    }
    public class CreateOrderRequest
    {
        public string UserId { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public PaymentDto Payment { get; set; } = null!;
    }

    public class OrderItemDto
    {
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class PaymentDto
    {
        public PaymentMethod Method { get; set; }
        public decimal Amount { get; set; }
    }

}
