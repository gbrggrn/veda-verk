using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VedaVerk.Shared.Enums;

namespace VedaVerk.Shared.DTOs
{
	public class CreateProductDTO
	{
		public int Id;

		[Required(ErrorMessage = "Produkten måste ha ett namn.")]
		public string? Name { get; set; }

		[Required(ErrorMessage = "Produkten måste ha en beskrivning.")]
		public string? Description { get; set; }

		[Required(ErrorMessage = "Produkten måste ha ett pris.")]
		public decimal Price { get; set; }

		[Required(ErrorMessage = "Ange total kapacitet.")]
		public int Capacity { get; set; }

		[Required(ErrorMessage = "Produkten behöver en representativ bild.")]
		public string? ImageUrl { get; set; }
		public ProductType Type { get; set; }
		public bool IsActive { get; set; }

		[Required(ErrorMessage = "Ange den första tiden då produkten är bokningsbar.")]
		public TimeSpan OpenTime { get; set; }

		[Required(ErrorMessage = "Ange den sista tiden då produkten är bokningsbar.")]
		public TimeSpan CloseTime { get; set; }

		[Required(ErrorMessage = "Ange bokningsintervall.")]
		public double IntervalMinutes { get; set; }

		[Required(ErrorMessage = "Ange hur många som kan boka i varje intervall.")]
		public int CapacityPerSlot { get; set; }
		public DateTime Created { get; set; }
		public DateTime LastUpdated { get; set; }
		public DateTime ActiveFrom { get; set; }
		public DateTime ActiveTo { get; set; }
	}
}
