namespace Ecom.Application.DTOs.Product
{
    public record ProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public List<PhotoDTO> Photos { get; set; }
        public string CategoryName { get; set; }
    }
}
