using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VedaVerk.Shared.DTOs
{
	public class CreateBookingDTO
	{
		[Required(ErrorMessage = "Du måste välja ett datum.")]
		public DateTime? BookingDate { get; set; }

		[Required(ErrorMessage = "Du måste välja en tid.")]
		public DateTime? BookingTime { get; set; }

		public int ProductId { get; set; }
		public string? CustomerName { get; set; }
		public string? CustomerEmail { get; set; }
		public string? CustomerPhone { get; set; }
		public int Quantity { get; set; }
	}
}