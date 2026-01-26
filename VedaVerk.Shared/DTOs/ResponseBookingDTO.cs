using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VedaVerk.Shared.DTOs
{
	public class ResponseBookingDTO
	{
		public int Id { get; set; }
		public DateTime BookingDate { get; set; }
		public TimeSpan BookingTime { get; set; }
		public int ProductId { get; set; }
		public string? CustomerName { get; set; }
		public string? CustomerEmail { get; set; }
		public string? CustomerPhone { get; set; }
		public int Quantity { get; set; }
	}
}
