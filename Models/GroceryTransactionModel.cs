using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryCashier.Models
{
    public class GroceryTransactionModel
    {
        public int Id { get; set; }
        public Guid StoreId { get; set; }
        public Guid CashierId { get; set; }
        public Guid CashierMachineID { get; set; }        
        public IEnumerable<GroceryItemModel> Items { get; set; }
        public ICollection<DiscountedItemModel> DiscountedItems { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxTotal { get; set; }
        public int PaymentTypeId { get; set; }
        public DateTime DateCreatedUTC => DateTime.UtcNow;
    }
}
