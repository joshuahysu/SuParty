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
        /// 大頭貼位置
        /// </summary>
        public string AvatarUrl { get; set; } = "";

    }
}