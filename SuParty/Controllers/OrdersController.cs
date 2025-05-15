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
        private readonly ILinePayService _linePayService; // 包裝 LINE Pay 呼叫的服務
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
                return BadRequest("購物車是空的");

            try
            {
                // 1. 建立訂單
                var order = await CreateOrderAsync(cartItems);

                // 2. 組成 LINE Pay 付款請求 DTO
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

                // 3. 呼叫 LINE Pay API 取得付款網址
                var paymentUrl = await _linePayService.RequestPaymentAsync(paymentRequestDto);

                // 4. 回傳訂單編號與付款連結
                return Ok(new CreateOrderResponse
                {
                    OrderNumber = order.Id,
                    PaymentUrl = paymentUrl
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "建立訂單失敗");
                return StatusCode(500, "建立訂單失敗，請稍後再試");
            }
        }

        // 這裡你可以放剛剛的 CreateOrderAsync 實作
        private async Task<Order> CreateOrderAsync(List<CartItemDto> cartItems)
        {
            // 取得商品資料
            var products = await _dbContext.ProductDatas
                .Where(p => cartItems.Select(ci => ci.ProductId).Contains(p.Id))
                .ToListAsync();

            if (products.Count == 0)
                throw new Exception("商品不存在");

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
