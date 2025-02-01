namespace FetchApi
{
    public class Item
    {
        public string ShortDescription { get; set; }
        public double Price { get; set; }

        public Item(string shortDescription, double price)
        {
            this.ShortDescription = shortDescription;
            this.Price = price;
        }
    }
}
