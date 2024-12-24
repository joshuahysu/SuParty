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
        public DbSet<Message> Messages { get; set; }
    }
    public class Message
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class UserData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string? Email { get; set; }
        public string NickName { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string Income { get; set; }
        public string Budget { get; set; }
    }

}