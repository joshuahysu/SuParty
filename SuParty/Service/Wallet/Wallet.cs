using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SuParty.Data;
using System.Data;

namespace SuParty.Service.Wallet
{
    public static class Wallet
    {
        public static (bool success, string message) UpdateWallet(this ApplicationDbContext dbContext, string id, decimal amount)
        {
            using var transaction = dbContext.Database.BeginTransaction(IsolationLevel.Serializable);

            // 這邊可以先選擇用條件更新，或先查再更新
            int rowsAffected = dbContext.Database.ExecuteSqlRaw(
                "UPDATE UserWallets SET Wallet = Wallet + @amount WHERE Id = @id AND Wallet + @amount >= 0",
                new[]
                {
                    new SqlParameter("@amount", amount),
                    new SqlParameter("@id", id)
                });

            if (rowsAffected > 0)
            {
                transaction.Commit();
                return (true, "扣款成功");
            }
            else
            {
                transaction.Rollback();
                return (false, "餘額不足或扣款失敗");
            }

        }
    }
}
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
