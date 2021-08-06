using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroceryCashier.Models;

namespace GroceryCashier.Service
{
    public interface IGroceryService
    {
        void UpdateItemCurrentPrice(string filePath);
        void UpdateItemCurrentPromotion(string filePath);
        void GetTransactionSubTotal(ref GroceryTransactionModel transaction);
        void GetTransactionTaxTotal(ref GroceryTransactionModel transaction);
        void PrintReceipt(GroceryTransactionModel transaction);
    }
}
