namespace CompComponentsDB.Models
{
    public class Component
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public string Supplier { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public string SupplyDate { get; set; } = "";
    }
}