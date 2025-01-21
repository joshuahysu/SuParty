using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SuParty.Data;
using TronNet.Protocol;

namespace SuParty.Service.Wallet
{
    public static class Wallet
    {
        public static (bool success,string message) UpdateWallet(this ApplicationDbContext dbContext,string id,decimal amount) {
            int rowsAffected = dbContext.Database.ExecuteSqlRaw(
            "UPDATE UserWallets SET Wallet = Wallet + @amount WHERE Id = @id AND Wallet + @amount >= 0",
            [
                new SqlParameter("@amount", amount),
                new SqlParameter("@id", id)
                ]);

            if (rowsAffected > 0)
            {
                return (true, "扣款成功");
            }
            else
            {
                return (true, "餘額不足或網路問題，扣款失敗");
            }       
        }
    }
}
