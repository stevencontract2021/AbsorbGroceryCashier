Notes:

-coded support for items charged by both pound and unit.
-coded support for promotion of BuyFourGetOneFree, BuyThreeForBundlePrice, DiscountedSale
-coded support for updating pricing and promotion from a file (CurrentItemPrice.txt and Promotion.txt)
-coded support for tax type (pst, gst etc) charged for all items
-Receipt Printout has cashier and machine and store Id and trasnaction time and transaction Id for tracing purpose
-CurrentItemPrice.txt has line by line of format [Item Name], [Item Id], [Item Price]
-Promotion.txt has line by line of format  [Item Name], [Item Id], [Discounted Type], [Discounted Price].  
 negative value in [Discounted Price] means no update on the [Discounted Price].
-there coule be some small precision lost due to two d.p in rounding in arithmetic.
-Some tested input and tested calls commented out in CheckOutCashier.cs main method.  
  You can uncomment them to test, or test the code with your own input.
-Some promotions and price are hardcoded as default.

