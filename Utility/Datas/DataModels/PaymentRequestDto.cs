
namespace SuParty.Data.DataModel
{
    public class PaymentRequestDto
    {
        public decimal Amount { get; set; }               // 總付款金額 (整數，單位新台幣)
        public string OrderId { get; set; }           // 訂單編號（你系統內唯一）
        public string ConfirmUrl { get; set; }        // 付款完成後 LINE Pay 會導回你的網址
        public string CancelUrl { get; set; }         // 付款取消後導回你的網址
        public List<PackageDto> Packages { get; set; }  // 商品組合（多商品時用）
    }

    public class PackageDto
    {
        public string Id { get; set; }                 // 包裹 ID (可隨便取)
        public decimal Amount { get; set; }                // 包裹金額
        public List<ProductDto> Products { get; set; }  // 商品清單
    }

    public class ProductDto
    {
        public string Name { get; set; }               // 商品名稱
        public int Quantity { get; set; }              // 數量
        public decimal Price { get; set; }                  // 單價
    }


}