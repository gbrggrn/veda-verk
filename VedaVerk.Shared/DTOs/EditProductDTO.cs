using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VedaVerk.Shared.Enums;

namespace VedaVerk.Shared.DTOs
{
	public class EditProductDTO
	{
		public string? Name { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public int Capacity { get; set; }
		public string? ImageUrl { get; set; }
		public ProductType Type { get; set; }
	}
}
