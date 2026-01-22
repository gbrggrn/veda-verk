using System.ComponentModel.DataAnnotations;
using VedaVerk.Shared.Enums;

namespace VedaVerk.Models.Enitites
{
	public class Product
	{
		public int Id { get; set; }
		[Required] public string Name { get; set; } = string.Empty;
		[Required] public string Description { get; set; } = string.Empty;
		[Required] public decimal Price { get; set; }
		[Required] public int Capacity { get; set; }
		public ProductType Type { get; set; }
		public string? ImageUrl { get; set; }
	}
}
