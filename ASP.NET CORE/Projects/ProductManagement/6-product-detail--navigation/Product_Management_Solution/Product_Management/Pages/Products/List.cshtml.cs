using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Product_Management_Classes;

namespace Product_Management.Pages.Products
{
    public class ListModel : PageModel
    {
        public string PageTitle { get; set; }
        public string ImageButtonTitle { 
            get {
                return "show";
            }
        }

        public List<Product> Products { get; private set; }
        public IProductCore ProductCore { get; }

        [BindProperty(SupportsGet =true)]
        public string SearchTerm { get; }

        public ListModel(IProductCore productCore)
        {
            ProductCore = productCore;
        }

        public void OnGet()
        {
            PageTitle = "Product Management Lists Page";
            Products = ProductCore.GetProducts();
        }
    }
}
