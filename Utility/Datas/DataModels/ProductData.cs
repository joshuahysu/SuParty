using System.ComponentModel.DataAnnotations;

namespace SuParty.Data.DataModel
{
    public class ProductData
    {
        public string Id { get; set; } = new Guid().ToString();

        [Required(ErrorMessage = "產品名稱是必填欄位。")]
        [StringLength(50, ErrorMessage = "產品名稱不能超過 50 個字元。")]
        public string Name { get; set; } = "";

        /// <summary>
        /// 簡介
        /// </summary>
        public string Introduction { get; set; } = "";
        [Required(ErrorMessage = "價格是必填欄位。")]
        public decimal Price { get; set; } =999999999;

        /// <summary>
        /// 圖片
        /// </summary>
        public List<string> Images { get; set; } = new();

        public string ProductType { get; set; } = "";

        public string? SalesId { get; set; }

        public List<UserData> Users { get; set; } = new(); // 多對多反向關係
    }
}