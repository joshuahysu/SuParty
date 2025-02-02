using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuParty.Data.DataModel;
using SuParty.Data.DataModel.RealEstate;

namespace SuParty.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<UserData> UserDatas { get; set; }
        public DbSet<UserWallet> UserWallets { get; set; }
        public DbSet<Tracking> Trackings { get; set; }
        public DbSet<ProductData> ProductDatas { get; set; }
        public DbSet<HouseData> HouseDatas { get; set; }
        
    }    
}