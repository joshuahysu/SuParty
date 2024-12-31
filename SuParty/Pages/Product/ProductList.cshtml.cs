using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data.DataModel;

namespace SuParty.Pages.Product
{
    public class ProductListModel : PageModel
    {
        public List<ProductData> productList = new();
        public void OnGet()
        {
        }
    }
}
