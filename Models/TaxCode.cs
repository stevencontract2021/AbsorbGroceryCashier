using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace GroceryCashier.Models
{
    public enum TaxCode
    {
        GST,
        PST,
        PST_SK,
        HST,
        HST_ON,
        RST,
        QST
    }

    public static class TaxCodes
    {      
        public static IReadOnlyDictionary<int, decimal> TaxCodeCol
            = new Dictionary<int, decimal>()
            {
                {(int)TaxCode.GST, (decimal)0.05 },
                {(int)TaxCode.PST, (decimal)0.07 },
                {(int)TaxCode.PST_SK, (decimal)0.06 },
                {(int)TaxCode.HST, (decimal)0.15 },
                {(int)TaxCode.HST_ON, (decimal)0.13 },
                {(int)TaxCode.RST, (decimal)0.07 },
                {(int)TaxCode.QST, (decimal)0.15 }
            };
    }
}
