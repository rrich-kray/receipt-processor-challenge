namespace FetchApi
{
    public class MorningReceipt
    {
        public string Retailer { get; set; }
        public string PurchaseDate { get; set; }
        public string PurchaseTime { get; set; }
        public double Total { get; set; }
        public List<Item> Items { get; set; }

        public MorningReceipt(string retailer, string PurchaseDate, string PurchaseTime, double total, List<Item> items)
        {
            this.Retailer = retailer;
            this.PurchaseDate = PurchaseDate;
            this.PurchaseTime = PurchaseTime;
            this.Total = total;
            this.Items = items;
        }
        public int CalculateTotalScore()
        {
            int score = 0;
            score += this.AlphaNumericScore();
            score += this.TotalCostScore();
            score += this.IsMultipleOf25Score();
            score += this.EveryTwoItemsScore();
            score += this.TrimmedLengthScore();
            score += this.IsPurchaseDayOddScore();
            score += this.TimeOfPurchaseScore();

            return score;
        }
        private int AlphaNumericScore() => this.Retailer.Count(char.IsLetterOrDigit);

        private int TotalCostScore() => this.Total % 1 == 0 ? 50 : 0;

        private int IsMultipleOf25Score() => this.Total % 0.25 == 0 ? 25 : 0;

        private int EveryTwoItemsScore() => (int)(Math.Floor((double)(this.Items.Count / 2)) * 5);

        private int TrimmedLengthScore()
        {
            int sumLengthOfTrimmedItems = 0;
            foreach (Item item in this.Items)
            {
                int itemDescriptionLength = item.ShortDescription.Trim().Length;
                if (itemDescriptionLength % 3 == 0)
                {
                    sumLengthOfTrimmedItems += (int)(Math.Ceiling(item.Price * 0.2));
                }
            }
            return sumLengthOfTrimmedItems;
        }

        private int IsPurchaseDayOddScore()
        {
            DateTime date = DateTime.Parse(this.PurchaseDate);
            return date.Day % 2 == 0 ? 0 : 6;
        }

        private int TimeOfPurchaseScore()
        {
            TimeOnly purchaseTime = TimeOnly.Parse(this.PurchaseTime);
            int greaterThan2 = purchaseTime.CompareTo(TimeOnly.Parse("2:00PM"));
            int earlierThan4 = purchaseTime.CompareTo(TimeOnly.Parse("4:00PM"));
            return greaterThan2 > 0 && earlierThan4 < 0 ? 10 : 0;
        }
    }
}
