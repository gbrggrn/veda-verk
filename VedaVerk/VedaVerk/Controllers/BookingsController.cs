using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VedaVerk.Models.Enitites;
using VedaVerk.Repositiories.Interfaces;
using VedaVerk.Shared;
using VedaVerk.Shared.DTOs;
using VedaVerk.Services;

namespace VedaVerk.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookingsController(IRepository<Product> productsrepository, IRepository<Booking> bookingsRepository, BookingService bookingService) : Controller
	{
		private readonly IRepository<Product> _productsRepository = productsrepository;
		private readonly IRepository<Booking> _bookingsRepository = bookingsRepository;
		private readonly BookingService _bookingService = bookingService;

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet("slots/{productId}")]
		public async Task<ActionResult<List<TimeSlotDTO>>> GetSlots(int productId, [FromQuery] DateTime date)
		{
			var slots = await _bookingService.GetSlotsAsync(productId, date);

			return Ok(slots);
		}

		[HttpGet("bookings-for-product/{productId}")]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<List<ResponseBookingDTO>>> GetBookingsForProduct(int productId)
		{
			return Ok(await _bookingService.GetBookingsForProductAsync(productId) ?? []);
		}

		[HttpGet("bookings-by-range/{productId}")]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<List<ResponseBookingDTO>>> GetBookingsByRange(
			int productId, 
			[FromQuery] DateTime start, 
			[FromQuery] DateTime end)
		{
			var bookings = await _bookingService.GetBookingsByRangeAsync(productId, start, end);
			return Ok(bookings);
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
				BookingDate = dto.BookingDate!.Value,
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

		[HttpPut("{id}")]
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

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			if (id <= 0)
				return BadRequest("ID was 0 or less");

			var booking = _bookingsRepository.GetByIdAsync(id);

			if (booking == null)
				return NotFound("Booking not found.");

			await _bookingsRepository.DeleteAsync(id);

			return Ok();
		}
	}
}
