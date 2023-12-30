namespace ModelLayer
{
	public class ModelProducts
	{
		public int ProductId { get; set; }
		public string? Name { get; set; }
		public string? Category { get; set; }
		public int? Price { get; set; }
		public string? Description { get; set; }
		public string? ImageUrl { get; set; }
		public DateTime ExpirationDate { get; set; }
		public DateTime StartDate { get; set; }
	}
}