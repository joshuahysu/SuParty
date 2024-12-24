using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SuParty.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<UserData> UserDatas { get; set; }
    }
    public class UserData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string? Email { get; set; }
        public string NickName { get; set; }
        public string Gender { get; set; }
        /// <summary>
        /// 自介
        /// </summary>
        public string Introduction { get; set; }
        public string Income { get; set; }
        public string Budget { get; set; }

        public string IG_Url { get; set; }
        public string ExtraUrl { get; set; }
        public List<string> ChatRooms { get; set; }

    }

}