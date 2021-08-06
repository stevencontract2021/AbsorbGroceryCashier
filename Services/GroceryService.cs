using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GroceryCashier.Models;
using GroceryCashier.UilityHelpers;
using System.IO;

namespace GroceryCashier.Service
{
    public class GroceryService: IGroceryService
    {
        private decimal GetSubTotalByWeight(GroceryItemModel item)     
            => (GroceryItems.ItemCol[item.ItemId].Price*item.Pound.Value).ToTwoDecimalPlaces();
                
        private decimal ApplyDiscount(ref GroceryTransactionModel transaction)
        {
            int freq = 0;
            decimal discounted = 0;
            transaction.DiscountedItems = new List<DiscountedItemModel>();
            var itemGroups = transaction.Items
                            .Where(t => !GroceryItems.ItemCol[t.ItemId].IsByPound)
                            .GroupBy(t => t.ItemId)
                            .Select(t => new { ItemId = t.Key, Count = t.Count()})
                            .OrderBy(x => x.ItemId);

            
            foreach (var itemGroup in itemGroups)
            {
                if (DiscountTypes.DiscountTypeCol.ContainsKey(itemGroup.ItemId))
                {                    
                    switch (DiscountTypes.DiscountTypeCol[itemGroup.ItemId])
                    {
                        case DiscountType.BuyFourGetOneFree:
                            freq = itemGroup.Count / (4 + 1);
                            if (freq > 0)
                            {
                                discounted = freq * GroceryItems.ItemCol[itemGroup.ItemId].Price;
                                transaction.DiscountedItems.Add(new DiscountedItemModel 
                                { ItemId = itemGroup.ItemId, Discounted = discounted, 
                                    DiscountType = DiscountType.BuyFourGetOneFree});
                                transaction.SubTotal -= discounted;
                            }
                            break;
                        case DiscountType.BuyThreeForBundlePrice:
                            freq = itemGroup.Count / 3;
                            if (freq > 0)
                            {
                                discounted = freq * 3*(GroceryItems.ItemCol[itemGroup.ItemId].Price - DiscountTypes.DiscountPriceCol[itemGroup.ItemId]);
                                transaction.DiscountedItems.Add(new DiscountedItemModel { ItemId = itemGroup.ItemId, 
                                    Discounted = discounted, 
                                    DiscountType = DiscountType.BuyThreeForBundlePrice
                                });
                                transaction.SubTotal -= discounted;
                            }
                            break;
                        case DiscountType.DiscountedSalePerItem:
                            discounted = itemGroup.Count * (GroceryItems.ItemCol[itemGroup.ItemId].Price - DiscountTypes.DiscountPriceCol[itemGroup.ItemId]);
                            transaction.DiscountedItems.Add(new DiscountedItemModel 
                            { ItemId = itemGroup.ItemId, Discounted = discounted, 
                                DiscountType = DiscountType.DiscountedSalePerItem });
                            transaction.SubTotal -= discounted;
                            break;
                        default:
                            freq = 0;
                            discounted = 0;
                            break;
                    }                    
                }

            }


            var itemByPoundGroups = transaction.Items
                            .Where(t => GroceryItems.ItemCol[t.ItemId].IsByPound)
                            .GroupBy(t => t.ItemId)
                            .Select(t => new { ItemId = t.Key, TotalPound = t.Sum(x=> x.Pound.Value)})
                            .OrderBy(x => x.ItemId);

            foreach (var itemByPound in itemByPoundGroups)
            {
                if (DiscountTypes.DiscountTypeCol.ContainsKey(itemByPound.ItemId))
                {
                    switch (DiscountTypes.DiscountTypeCol[itemByPound.ItemId])
                    {
                        case DiscountType.DiscountedSalePerPound:
                            discounted = itemByPound.TotalPound * (GroceryItems.ItemCol[itemByPound.ItemId].Price - DiscountTypes.DiscountPriceCol[itemByPound.ItemId]);
                            transaction.DiscountedItems.Add(new DiscountedItemModel 
                            { ItemId = itemByPound.ItemId, Discounted = discounted, 
                                DiscountType = DiscountType.DiscountedSalePerPound });
                            transaction.SubTotal -= discounted;
                            break;
                        default:
                            freq = 0;
                            discounted = 0;
                            break;
                    }
                }

            }

            return transaction.SubTotal.ToTwoDecimalPlaces();

        }

        public void GetTransactionSubTotal(ref GroceryTransactionModel transaction)
        {            
            foreach(var item in transaction.Items)
            {
                if (GroceryItems.ItemCol[item.ItemId].IsByPound)
                    transaction.SubTotal += GetSubTotalByWeight(item);
                else
                    transaction.SubTotal += GroceryItems.ItemCol[item.ItemId].Price;
            }

            ApplyDiscount(ref transaction);
        }
        public void GetTransactionTaxTotal(ref GroceryTransactionModel transaction)
        {
            decimal total = 0;
            var discountedIds = transaction
                .DiscountedItems 
                .Select(s => s.ItemId).ToList();

            foreach (var item in transaction.Items)
            {
                if (!discountedIds.Contains(item.ItemId) || !DiscountTypes.DiscountPriceCol.ContainsKey(item.ItemId))
                {
                    total += GroceryItems.ItemCol[item.ItemId].Price * GroceryItems.ItemCol[item.ItemId].TaxCodes.Sum(x => TaxCodes.TaxCodeCol[x]);
                }
                else
                {
                    if (GroceryItems.ItemCol[item.ItemId].IsByPound)
                        total += item.Pound.Value * DiscountTypes.DiscountPriceCol[item.ItemId] * GroceryItems.ItemCol[item.ItemId].TaxCodes.Sum(x => TaxCodes.TaxCodeCol[x]);
                    else
                        total += DiscountTypes.DiscountPriceCol[item.ItemId] * GroceryItems.ItemCol[item.ItemId].TaxCodes.Sum(x => TaxCodes.TaxCodeCol[x]);
                }
            }

             decimal buyfourgetonetotal = transaction.DiscountedItems
                 .Where(x => x.DiscountType == DiscountType.BuyFourGetOneFree)
                 .Sum(y => y.Discounted * GroceryItems.ItemCol[y.ItemId].TaxCodes.Sum(x => TaxCodes.TaxCodeCol[x]));


            total -= buyfourgetonetotal;

            transaction.TaxTotal = total.ToTwoDecimalPlaces();
        }

        public void PrintReceipt(GroceryTransactionModel transaction)
        {
            foreach (var prop in transaction.GetType().GetProperties())
            {
                if (!prop.Name.Contains("Items"))
                {
                    if (prop.Name.Contains("PaymentTypeId"))
                        Console.WriteLine("Payment Type: " + Enum.Parse(typeof(PaymentType), prop.GetValue(transaction).ToString()));
                    else
                        Console.WriteLine(prop.Name + ": " + prop.GetValue(transaction));
                }
            }

            foreach (var item in transaction.Items)
            {
                Console.WriteLine(GroceryItems.ItemCol[item.ItemId].Name + ": "
                    + GroceryItems.ItemCol[item.ItemId].Price.ToDollarString()
                    + (GroceryItems.ItemCol[item.ItemId].IsByPound ? " per pound" : string.Empty));
            }

            foreach (var discountedItem in transaction.DiscountedItems)
                Console.WriteLine(GroceryItems.ItemCol[discountedItem.ItemId].Name
                    + " has saving of $" + discountedItem.Discounted
                    + " for the promotion of " + discountedItem.DiscountType);


            Console.WriteLine("Total: " + (transaction.SubTotal + transaction.TaxTotal).ToDollarString());

        }

        public void UpdateItemCurrentPrice(string filePath)
        {
            string oneLine;

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var sr = new StreamReader(fs))
                {
                    while ((oneLine = sr.ReadLine()) != null)
                    {
                        string[] oneItemUpdate = oneLine.Split(",", StringSplitOptions.RemoveEmptyEntries);
                        GroceryItems.ItemCol[int.Parse(oneItemUpdate[1])].Price = decimal.Parse(oneItemUpdate[2]);
                    }

                }
            }

        }

        public void UpdateItemCurrentPromotion(string filePath)
        {
            string oneLine;

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var sr = new StreamReader(fs))
                {
                    while ((oneLine = sr.ReadLine()) != null)
                    {
                        string[] oneItemUpdate = oneLine.Split(",", StringSplitOptions.RemoveEmptyEntries);
                        if (DiscountTypes.DiscountTypeCol.ContainsKey(int.Parse(oneItemUpdate[1])))
                            DiscountTypes.DiscountTypeCol[int.Parse(oneItemUpdate[1])] = (DiscountType)Enum.Parse(typeof(DiscountType),oneItemUpdate[2]);
                        else
                            DiscountTypes.DiscountTypeCol.Add(int.Parse(oneItemUpdate[1]), (DiscountType)Enum.Parse(typeof(DiscountType), oneItemUpdate[2]));

                        if (decimal.Parse(oneItemUpdate[3]) > 0.00m)
                            DiscountTypes.DiscountPriceCol[int.Parse(oneItemUpdate[1])] = decimal.Parse(oneItemUpdate[3]);
                    }

                }
            }

        }

    }
}