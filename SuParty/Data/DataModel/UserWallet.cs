using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SuParty.Data.DataModel
{
    public class UserWallet
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Email { get; set; } = "";
        public List<string> Referrers { get; set; } = new();
        public Decimal Wallet { get; set; } = 0;

    }
}