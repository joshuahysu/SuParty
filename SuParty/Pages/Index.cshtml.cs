using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace SuParty.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resources = assembly.GetManifestResourceNames();
            foreach (var resource in resources)
            {
                Console.WriteLine(resource);
            }
        }
    }
}
