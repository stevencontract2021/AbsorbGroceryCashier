using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryCashier.Models
{
    public enum DiscountType
    {
        BuyFourGetOneFree,
        BuyThreeForBundlePrice,
        DiscountedSalePerItem,
        DiscountedSalePerPound
    }    

    public static class DiscountTypes
    {
        public static IDictionary<int, DiscountType> DiscountTypeCol
            = new Dictionary<int, DiscountType>()
            {
                //{(int)Item.Cheerios, DiscountType.BuyFourGetOneFree},
                {(int)Item.Detergent, DiscountType.DiscountedSalePerItem},
                {(int)Item.Apples, DiscountType.DiscountedSalePerPound},
                {(int)Item.Oranges, DiscountType.BuyThreeForBundlePrice}
            };

        public static IDictionary<int, decimal> DiscountPriceCol
            = new Dictionary<int, decimal>()
            {
                {(int) Item.Detergent, 2.00m},
                {(int) Item.Apples, 0.90m},
                {(int) Item.Oranges, 3.00m},
            };
    }

    public class DiscountedItemModel
    {
        public int ItemId { get; set; }
        public decimal Discounted { get; set; }
        public DiscountType DiscountType { get; set; }

    }

   
   
}
