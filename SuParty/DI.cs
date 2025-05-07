using Google.Api;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SuParty.Data;
using SuParty.Middleware;
using TronNet;

namespace SuParty
{
    public static class DI
    {
        public static IServiceProvider ServiceProvider { get; set; }
    }
}