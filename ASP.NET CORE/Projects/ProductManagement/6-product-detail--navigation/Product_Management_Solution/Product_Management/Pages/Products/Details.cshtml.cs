using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Product_Management_Classes;

namespace Product_Management.Pages.Products
{
    public class DetailsModel : PageModel
    {
        public Product Product { get; set;  }

        public IProductCore ProductCore { get; set; }

        public DetailsModel(IProductCore productCore)
        {
            this.ProductCore = productCore;
        }

        public IActionResult OnGet(string productId)
        {
            Product = ProductCore.GetProductById(int.Parse(productId));
            if (string.IsNullOrWhiteSpace(productId))
            {
                return Page();
            }
            return Page();
        }
    }
}
