using System;
using GroceryCashier.Service;
using GroceryCashier.Models;
using System.Collections.Generic;
using System.IO;

namespace AbsorbGroceryCashier
{
    class CheckOutCashier
    {
        static void Main(string[] args)
        {
            var transaction = new GroceryTransactionModel() 
            { 
                Id = 1,
                PaymentTypeId = (int)PaymentType.Cash, 
                StoreId = new Guid("955C5E0E-DE53-42CE-8CB5-72CB6DF28B21"),
                CashierId = new Guid("11A03BF7-7B44-4A73-B1AA-0E894F6C41CF"),
                CashierMachineID = new Guid("F318C756-4E64-4110-8E9E-80CC0229F149"),
                //Items = new List<GroceryItemModel>() {
                //    new GroceryItemModel { ItemId = (int)Item.Oranges },
                //    new GroceryItemModel { ItemId = (int)Item.Oranges },
                //    new GroceryItemModel { ItemId = (int)Item.Oranges },
                //    new GroceryItemModel { ItemId = (int)Item.Oranges }}
                Items = new List<GroceryItemModel>() {
                    new GroceryItemModel { ItemId = (int)Item.Cheerios },
                    new GroceryItemModel { ItemId = (int)Item.Cheerios },
                    new GroceryItemModel { ItemId = (int)Item.Cheerios },
                    new GroceryItemModel { ItemId = (int)Item.Cheerios },
                    new GroceryItemModel { ItemId = (int)Item.Cheerios } }
                //Items = new List<GroceryItemModel>() {
                //    new GroceryItemModel { ItemId = (int)Item.Detergent },
                //    new GroceryItemModel { ItemId = (int)Item.Detergent },
                //    new GroceryItemModel { ItemId = (int)Item.Detergent }

                //Items = new List<GroceryItemModel>() { new GroceryItemModel { ItemId = (int)Item.Apples, Pound = 2 } }
            };

            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

            var groceryService = new GroceryService();
           // groceryService.UpdateItemCurrentPrice(Path.Combine(projectDirectory, "CurrentItemPrice.txt"));
            //groceryService.UpdateItemCurrentPromotion(Path.Combine(projectDirectory, "Promotion.txt"));
            groceryService.GetTransactionSubTotal(ref transaction);
            groceryService.GetTransactionTaxTotal(ref transaction);
            groceryService.PrintReceipt(transaction);
        }
    }
}
