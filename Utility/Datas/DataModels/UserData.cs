namespace SuParty.Data.DataModel
{
    public class UserData
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public DateTime Birthday { get; set; }
        public string? Email { get; set; } = "";

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; } = "";

        /// <summary>
        /// 小名
        /// </summary>
        public string NickName { get; set; } = "";
        /// <summary>
        /// 性別
        /// </summary>
        public string Gender { get; set; } = "";

        /// <summary>
        /// 家用電話
        /// </summary>
        public string HousePhone { get; set; } = "";

        public string Phone { get; set; } = "";

        /// <summary>
        /// 會員等級
        /// </summary>
        public int MembershipLevel { get; set; }

        /// <summary>
        /// 會員到期日
        /// </summary>
        public DateTime MembershipExpirationDate { get; set; }= DateTime.Now.AddDays(-1);
        /// <summary>
        /// 大頭貼位置
        /// </summary>
        public string AvatarUrl { get; set; } = "";

        /// <summary>
        /// 自介
        /// </summary>
        public string Introduction { get; set; } = "";
        /// <summary>
        /// 收入
        /// </summary>
        public string Income { get; set; } = "";

        /// <summary>
        /// 預算
        /// </summary>
        public string Budget { get; set; } = "";
        public string Line_Url { get; set; } = "";
        public string IG_Url { get; set; } = "";
        public string ExtraUrl { get; set; } = "";

        public List<string> ChatRooms { get; set; } = new();

        #region 產品資料
        public List<ProductData> ShoppingCart { get; set; } = new();

        public List<string> Store { get; set; } = new();
        #endregion

        #region 私人資料
        /// <summary>
        /// 儲存使用者的身高（以字串格式表示）。
        /// </summary>
        public string Height { get; set; } = "";

        /// <summary>
        /// 儲存使用者的體重（以字串格式表示）。
        /// </summary>
        public string Weight { get; set; } = "";

        /// <summary>
        /// 儲存使用者的胸圍（以字串格式表示）。
        /// </summary>
        public string Bust { get; set; } = "";

        /// <summary>
        /// 儲存使用者的腰圍（以字串格式表示）。
        /// </summary>
        public string Waist { get; set; } = "";

        /// <summary>
        /// 儲存使用者的臀圍（以字串格式表示）。
        /// </summary>
        public string Hips { get; set; } = "";

        #endregion
    }
}