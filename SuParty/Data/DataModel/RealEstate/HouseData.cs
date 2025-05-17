using Microsoft.AspNetCore.Mvc;
using SuParty.Data.DataModel.RealEstate.Enum;
using System.ComponentModel.DataAnnotations;
using static TronNet.Protocol.TransactionInfo.Types;

namespace SuParty.Data.DataModel.RealEstate
{
    public class HouseData
    {
        public string Id { get; set; } = new Guid().ToString();

        /// <summary>
        /// 廣告排序
        /// </summary>

        [Required(ErrorMessage = "產品名稱是必填欄位。")]
        [StringLength(50, ErrorMessage = "產品名稱不能超過 50 個字元。")]
        public string Name { get; set; } = "house";
        /// <summary>
        /// 建案名
        /// </summary>
        public string DevelopmentName { get; set; } = "";

        /// <summary>
        /// 簡介
        /// </summary>
        public string Introduction { get; set; } = "是個好房子";
        [Required(ErrorMessage = "價格是必填欄位。")]
        public decimal Price { get; set; } = 999999999;

        /// <summary>
        /// 完工日期
        /// </summary>
        public DateTime Year { get; set; }= DateTime.Now;


        /// <summary>
        /// 圖片
        /// </summary>
        public List<string> Images { get; set; } = new();
        [Required(AllowEmptyStrings = true)]
        public string VideoUrl { get; set; } = "";
        
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; } = "";
        public string ProductType { get; set; } = "";

        public CityEnum City { get; set; }
        public HouseTypeEnum HouseType { get; set; }

        /// <summary>
        /// 權狀
        /// </summary>
        public float Space { get; set; } = 0;
        /// <summary>
        /// 主建
        /// </summary>
        public float RealSpace { get; set; } = 0;
        /// <summary>
        /// 土地坪數
        /// </summary>
        public float LotSize { get; set; } = 0;
        /// <summary>
        /// 共用部分(坪)
        /// </summary>
        public float CommonArea { get; set; } = 0;

        /// <summary>
        /// 單價
        /// </summary>
        public float PricePerPing { get; set; } = 0;

        /// <summary>
        /// 租金
        /// </summary>
        public float Rent { get; set; } = 0;

        /// <summary>
        /// 房間數量
        /// </summary>
        public int RoomCount { get; set; } = 0;
        public int RestroomCount { get; set; } = 0;
        /// <summary>
        /// 客廳數
        /// </summary>
        public int LivingRoomCount { get; set; } = 0;

        public int ParkingSpaceCount { get; set; } = 0;
        public int ParkingSpace { get; set; } = 0;
        public ParkingSpaceTypeEnum ParkingSpaceType { get; set; } = 0;
        
        /// <summary>
        /// 樓層
        /// </summary>
        public int Floor { get; set; } = 1;

        /// <summary>
        /// 總樓層
        /// </summary>
        public int TotalFloor { get; set; } = 1;

        /// <summary>
        /// 賣家ID
        /// </summary>
        public string SalesId { get; set; } = "";

        /// <summary>
        /// 廣告排序
        /// </summary>
        public int Index { get; set; } = 1;

        /// <summary>
        /// 管理費
        /// </summary>
        public int MaintenanceFee{ get; set; } =0;

        /// <summary>
        /// 是否自售
        /// </summary>
        public string Seller { get; set; } = "";
    }
}