using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SuParty.Data.DataModel.RealEstate
{
    public class HouseData
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
        public decimal Price { get; set; } = 999999999;

        /// <summary>
        /// 圖片
        /// </summary>
        public List<string> Images { get; set; } = new();

        public string VideoUrl { get; set; } = "";
        public string Address { get; set; } = "";
        public string ProductType { get; set; } = "";

        public City City { get; set; }

        public float Space { get; set; } = 0;
        public float PricePerPing { get; set; } = 0;

        public int RoomCount { get; set; } = 0;
        public int RestroomCount { get; set; } = 0;
        public int LivingRoomCount { get; set; } = 0;
                
        public int ParkingSpaceCount { get; set; } = 0;
        public int ParkingSpace { get; set; } = 0;

        public int Floor { get; set; } = 1;

        public string SalesId { get; set; } = "";

        public int Index { get; set; } = 1;


    }
}