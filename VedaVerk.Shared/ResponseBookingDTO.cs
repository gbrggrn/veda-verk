using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VedaVerk.Shared
{
	public class ResponseBookingDTO
	{
		public DateTime BookingDate { get; set; }
		public int ProductId { get; set; }
		public string? CustomerName { get; set; }
	}
}
