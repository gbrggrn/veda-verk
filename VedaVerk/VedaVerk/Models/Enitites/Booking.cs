using System.ComponentModel.DataAnnotations;

namespace VedaVerk.Models.Enitites
{
	public class Booking
	{
		public int Id { get; set; }

		// Booking details
		[Required] public DateTime BookingDate { get; set; }
		[Required] public Product? Product { get; set; }
		[Required] public int Quantity { get; set; }

		// Pickup time
		[Required] public DateTime BookingTime { get; set; }

		// Customer details
		[Required] public string CustomerName { get; set; } = string.Empty;
		[Required] public string CustomerEmail { get; set; } = string.Empty;
		[Required] public string CustomerPhone { get; set; } = string.Empty;

		// Status
		public bool IsCancelled { get; set; }

		// Foreign key
		public int ProductId { get; set; }

		// Secret token for cancellation
		public Guid SecretToken { get; set; } = Guid.NewGuid();
	}
}
