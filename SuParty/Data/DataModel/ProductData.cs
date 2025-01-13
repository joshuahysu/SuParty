using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SuParty.Data.DataModel
{
    public class ProductData
    {
        public string Id { get; set; } = new Guid().ToString();

        [Required(ErrorMessage = "產品名稱是必填欄位。")]
        [StringLength(50, ErrorMessage = "產品名稱不能超過 50 個字元。")]
        public string Name { get; set; } = "test";

        /// <summary>
        /// 簡介
        /// </summary>
        public string Introduction { get; set; } = "";
        [Required(ErrorMessage = "價格是必填欄位。")]
        public decimal Price { get; set; } =999999999;

        /// <summary>
        /// 圖片
        /// </summary>
        public string Image { get; set; } = "";

        public string ProductType { get; set; } = "";
    }
}