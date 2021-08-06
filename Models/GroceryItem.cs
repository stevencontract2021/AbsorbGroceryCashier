using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace GroceryCashier.Models
{
    public class GroceryItem
    {        
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string BarCode { get; set; }        
        public decimal Price { get; set; }
        public IEnumerable<int> TaxCodes { get; set; }
        public bool IsByPound { get; set; }
        
    }
   
    public static class GroceryItems
    {
        public static IDictionary<int, GroceryItem> ItemCol
            = new Dictionary<int, GroceryItem>()
            {
                { (int)Item.Cheerios, new GroceryItem {
                    ItemId = (int)Item.Cheerios, Name = nameof(Item.Cheerios), BarCode = "789252555XXAA",
                    TaxCodes = new List<int> { (int) TaxCode.PST },
                    Price = (decimal) 6.99m } },
                 { (int)Item.Detergent, new GroceryItem {
                    ItemId = (int)Item.Detergent, Name = nameof(Item.Detergent), BarCode = "123252578XXAA",
                    TaxCodes = new List<int> { (int)TaxCode.GST, (int) TaxCode.PST },
                    Price = (decimal) 5.48m } },
                 { (int)Item.Apples, new GroceryItem {
                    ItemId = (int)Item.Apples, Name = nameof(Item.Apples), BarCode = "777666578XXAA",
                    TaxCodes = new List<int> { (int) TaxCode.PST }, IsByPound = true,
                    Price = (decimal) 2.49m } },
                  { (int)Item.Oranges, new GroceryItem {
                    ItemId = (int)Item.Oranges, Name = nameof(Item.Oranges), BarCode = "777666578XXAB",
                    TaxCodes = new List<int> { (int) TaxCode.PST },
                    Price = (decimal) 3.20m } }
            };

    }

}
