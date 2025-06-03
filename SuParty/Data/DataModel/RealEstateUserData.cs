
namespace SuParty.Data.DataModel
{
    public class RealEstateUserData: UserData
    {   

        /// <summary>
        /// 經紀業證號
        /// </summary>
        public string Brokerage { get; set; } = "";


        #region 產品資料

        #endregion

        #region 房產資料
        public List<TrackingObject> TraceRealEstates { get; set; } = new();

        #endregion
       
    }
}