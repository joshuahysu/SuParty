using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SuParty.Data.DataModel
{
    /// <summary>
    /// 追蹤訂閱
    /// </summary>
    public class Tracking
    {
        public string Id { get; set; } = "";

        /// <summary>
        /// 喜愛程度
        /// </summary>
        public int LoveScore { get; set; }
        /// <summary>
        /// 追蹤列表
        /// </summary>
        public List<TrackingObject> TrackingList { get; set; } = new();

    }
    public class TrackingObject
    {
        public string Id { get; set; } = "";

        /// <summary>
        /// 喜愛程度
        /// </summary>
        public int LoveScore { get; set; }
    }
}