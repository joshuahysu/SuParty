using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuParty.Data.DataModel;
using SuParty.Data.DataModel.RealEstate;
using SuParty.Service.Referrer;
using Utility.Service.Product.enums;

namespace SuParty.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<RealEstateUserData> UserDatas { get; set; }
        public DbSet<UserWallet> UserWallets { get; set; }
        public DbSet<Tracking> Trackings { get; set; }
        public DbSet<ProductData> ProductDatas { get; set; }
        public DbSet<HouseData> HouseDatas { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ReferrerMember> ReferrerMembers { get; set; }
        
    }    
}