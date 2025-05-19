
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SuParty.Data.DataModel.RealEstate;
using System.Security.Claims;

namespace SuParty.Unit
{
    public class WebUnit:PageModel
    {
        public IActionResult IsLogin(ClaimsPrincipal User)
        {

            if (User.Identity.IsAuthenticated)
            {
            }
            else
            {
                return RedirectToPage("/Account/Login");
            }
            return RedirectToPage("/Account/Login");
        }
    }
}
