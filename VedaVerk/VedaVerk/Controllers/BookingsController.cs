using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VedaVerk.Models.Enitites;
using VedaVerk.Repositiories.Interfaces;
using VedaVerk.Shared;
using VedaVerk.Shared.DTOs;

namespace VedaVerk.Controllers
{
	public class BookingsController(IRepository<Product> productsrepository, IRepository<Booking> bookingsRepository) : Controller
	{
		private readonly IRepository<Product> _productsRepository = productsrepository;
		private readonly IRepository<Booking> _bookingsRepository = bookingsRepository;

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Create(CreateBookingDTO dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (dto == null)
				return BadRequest("Booking was null.");

			var booking = new Booking
			{
				BookingDate = dto.BookingDate,
				ProductId = dto.ProductId,
				CustomerName = dto.CustomerName ?? string.Empty,
				CustomerEmail = dto.CustomerEmail ?? string.Empty,
				CustomerPhone = dto.CustomerPhone ?? string.Empty
			};

			await _bookingsRepository.AddAsync(booking);

			var responseDto = new ResponseBookingDTO
			{
				BookingDate = booking.BookingDate,
				ProductId = booking.ProductId,
				CustomerName = booking.CustomerName,
			};

			return Ok(responseDto);
		}

		[HttpDelete("{íd}")]
		public async Task<IActionResult> Cancel(int id, [FromQuery] Guid token)
		{
			var booking = _bookingsRepository.GetByIdAsync(id).Result;

			if (booking == null)
				return NotFound("Booking not found.");

			if (booking.SecretToken != token)
				return Unauthorized("Invalid token.");

			booking.IsCancelled = true;

			await _bookingsRepository.UpdateAsync(booking);

			return Ok("Booking cancelled successfully.");
		}
	}
}
