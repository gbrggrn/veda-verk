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

		[Required(ErrorMessage = "Du måste ange ett namn.")]
		[MaxLength(50)]
		public string? CustomerName { get; set; }

		[Required(ErrorMessage = "Du måste ange en e-postadress")]
		[EmailAddress(ErrorMessage = "Ange en korrekt e-postadress")]
		public string? CustomerEmail { get; set; }

		[Required(ErrorMessage = "Du måste ange ett telefonnummer.")]
		[Phone(ErrorMessage = "Du måste ange ett telefonnummer.")]
		public string? CustomerPhone { get; set; }

		[Required(ErrorMessage = "Kvantitet kan inte vara noll.")]
		public int Quantity { get; set; }
	}
}