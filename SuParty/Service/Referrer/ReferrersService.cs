using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SuParty.Data;
using SuParty.Service.Wallet;

namespace SuParty.Service.Referrer
{
    public class ReferrersService
    {
        private readonly ApplicationDbContext _dbContext;

        public ReferrersService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void onlyOne(string id,decimal amount,decimal b) {
            var user=_dbContext.UserWallets.Find(id);
            var ReferrerId=user.Referrers.FirstOrDefault();
            if (!ReferrerId.IsNullOrEmpty())
            {
                var Referrer = _dbContext.UserWallets.Find(ReferrerId);
                _dbContext.UpdateWallet(ReferrerId, amount * b);
            }
        }


        public void t(string id)
        {
            //誰可以分
        }
    }
}
