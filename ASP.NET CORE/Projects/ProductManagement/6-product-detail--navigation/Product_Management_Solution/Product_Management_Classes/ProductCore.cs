using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product_Management_Classes
{
    public interface IProductCore
    {
        List<Product> GetProducts();
        Product GetProductById(int productId);
    }

    public class ProductCore : IProductCore
    {
        List<Product> products = new List<Product>
       {
            new Product {  productId= 2,
          productName= "Garden Cart",
          productCode= "GDN-0023",
          releaseDate= "March 18, 2016",
          description= "15 gallon capacity rolling garden cart",
          price= 32.99,
          starRating= 4.2,
          imageUrl= "http=//openclipart.org/image/300px/svg_to_png/58471/garden_cart.png" },

        new Product
        {
            productId= 5,
          productName= "Hammer",
          productCode= "TBX-0048",
          releaseDate= "May 21, 2016",
          description= "Curved claw steel hammer",
          price= 8.9,
          starRating= 4.8,
          imageUrl= "http=//openclipart.org/image/300px/svg_to_png/73/rejon_Hammer.png"
        }
       };
           

public List<Product> GetProducts()
        {
            return products;
        }

        public Product GetProductById(int productId)
        {
            return products.FirstOrDefault(x => x.productId == productId);
        }
    }
}
