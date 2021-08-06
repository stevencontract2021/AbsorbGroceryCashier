using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroceryCashier.UilityHelpers
{
    public static class DecimalOperation
    {
        public static decimal ToTwoDecimalPlaces(this decimal input)
        {
            return Math.Round(input, 2);
        }

        public static string ToDollarString(this decimal input)
        {
            return $"$ {input}";
        }
    }
}