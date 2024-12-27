using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SuParty.Data.DataModel
{
    public class Tracking
    {
        public string Id { get; set; } = "";
        /// <summary>
        /// 追蹤列表
        /// </summary>
        public List<string> TrackingList { get; set; } = new();

    }
}