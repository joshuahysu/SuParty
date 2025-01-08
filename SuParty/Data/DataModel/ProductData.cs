using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SuParty.Data.DataModel
{
    public class ProductData
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";

        /// <summary>
        /// 簡介
        /// </summary>
        public string Introduction { get; set; } = "";

        public decimal Price { get; set; } =0;

        /// <summary>
        /// 圖片
        /// </summary>
        public string Image { get; set; } = "";

        public string ProductType { get; set; } = "";
    }
}