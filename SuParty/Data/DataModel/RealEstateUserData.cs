using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SuParty.Data.DataModel
{
    public class RealEstateUserData: UserData
    {
   

        /// <summary>
        /// 經紀業
        /// </summary>
        public string Brokerage { get; set; } = "";


        #region 產品資料

        #endregion

        #region 房產資料
        public List<string> TraceRealEstates { get; set; } = new();

        #endregion
       
    }
}