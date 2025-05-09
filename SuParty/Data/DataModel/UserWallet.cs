
namespace SuParty.Data.DataModel
{
    public class UserWallet
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Email { get; set; } = "";
        public List<string> Referrers { get; set; } = new();
        
        /// <summary>
        /// 錢包
        /// </summary>
        public Decimal Wallet { get; set; } = 0;

        /// <summary>
        /// 點數
        /// </summary>
        public Decimal Points { get; set; } = 0;

        /// <summary>
        /// 家貓幣
        /// </summary>
        public Decimal HouseCatTokens { get; set; } = 0;       

    }
}