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
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public int Capacity { get; set; }
		public string? ImageUrl { get; set; }
		public ProductType Type { get; set; }
		public bool IsActive { get; set; }
		public TimeSpan OpenTime { get; set; }
		public TimeSpan CloseTime { get; set; }
		public double IntervalMinutes { get; set; }
		public int CapacityPerSlot { get; set; }
		public DateTime Created { get; set; }
		public DateTime LastUpdated { get; set; }
		public DateTime ActiveFrom { get; set; }
		public DateTime ActiveTo { get; set; }
	}
}
