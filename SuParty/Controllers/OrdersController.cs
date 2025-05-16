using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuParty.Data;
using SuParty.Data.DataModel;
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
        private readonly ApplicationDbContext _dbContext;
        private readonly ILinePayService _linePayService; // �]�� LINE Pay �I�s���A��
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ApplicationDbContext dbContext, ILinePayService linePayService, ILogger<OrdersController> logger)
        {
            _dbContext = dbContext;
            _linePayService = linePayService;
            _logger = logger;
        }

        public class CartItemDto
        {
            public string ProductId { get; set; }
            public int Quantity { get; set; }
        }

        public class CreateOrderResponse
        {
            public string OrderNumber { get; set; }
            public string PaymentUrl { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] List<CartItemDto> cartItems)
        {
            if (cartItems == null || !cartItems.Any())
                return BadRequest("�ʪ����O�Ū�");

            try
            {
                // 1. �إ߭q��
                var order = await CreateOrderAsync(cartItems);

                // 2. �զ� LINE Pay �I�ڽШD DTO
                var paymentRequestDto = new PaymentRequestDto
                {
                    Amount = order.TotalAmount,
                    OrderId = order.Id,
                    ConfirmUrl = "https://yourdomain.com/api/linepay/confirm",
                    CancelUrl = "https://yourdomain.com/api/linepay/cancel",
                    Packages = new List<PackageDto>
                {
                    new PackageDto
                    {
                        Id = "package1",
                        Amount = order.TotalAmount,
                        Products = order.Items.Select(i =>
                        {
                            var product = _dbContext.ProductDatas.First(p => p.Id == i.ProductId);
                            return new ProductDto
                            {
                                Name = product.Name,
                                Quantity = i.Quantity,
                                Price = i.Price
                            };
                        }).ToList()
                    }
                }
                };

                // 3. �I�s LINE Pay API ���o�I�ں��}
                var paymentUrl = await _linePayService.RequestPaymentAsync(paymentRequestDto);

                // 4. �^�ǭq��s���P�I�ڳs��
                return Ok(new CreateOrderResponse
                {
                    OrderNumber = order.Id,
                    PaymentUrl = paymentUrl
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "�إ߭q�楢��");
                return StatusCode(500, "�إ߭q�楢�ѡA�еy��A��");
            }
        }

        // �o�̧A�i�H���誺 CreateOrderAsync ��@
        private async Task<Order> CreateOrderAsync(List<CartItemDto> cartItems)
        {
            // ���o�ӫ~���
            var products = await _dbContext.ProductDatas
                .Where(p => cartItems.Select(ci => ci.ProductId).Contains(p.Id))
                .ToListAsync();

            if (products.Count == 0)
                throw new Exception("�ӫ~���s�b");

            var order = new Order
            {
                Id = Guid.NewGuid().ToString("N").ToUpper(),
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                Items = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            foreach (var item in cartItems)
            {
                var product = products.First(p => p.Id == item.ProductId);
                decimal itemTotalPrice = product.Price * item.Quantity;

                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    Price = product.Price
                });

                totalAmount += itemTotalPrice;
            }

            order.TotalAmount = totalAmount;

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            return order;
        }
    }

}
