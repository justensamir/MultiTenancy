namespace MultiTenancy.Dtos
{
    public class CreateProductDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Rate { get; set; }
    }
}
