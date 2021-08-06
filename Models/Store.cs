using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryCashier.Models
{
    public class Store
    {
        public Guid Id { get; set; }
        public string AddressLine1 {get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public int ProvinceId { get; set; }        
    }
}
