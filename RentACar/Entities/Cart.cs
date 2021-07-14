namespace RentACar.Entities
{
    public class Cart
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public decimal Price { get; set; }

        public decimal PriceDiscounted { get; set; }
    }
}