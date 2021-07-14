namespace RentACar.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal PriceDiscounted { get; set; }

        public bool IsAvailable { get; set; }

        public string ImageVehicle { get; set; }

        public CssStyle Style { get; set; }
    }
}
