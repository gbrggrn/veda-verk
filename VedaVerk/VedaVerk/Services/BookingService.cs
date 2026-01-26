using Microsoft.EntityFrameworkCore;
using VedaVerk.Data;
using VedaVerk.Models.Enitites;
using VedaVerk.Repositiories.Interfaces;
using VedaVerk.Shared.DTOs;

namespace VedaVerk.Services
{
	public class BookingService (IRepository<Product> productRepository, IRepository<Booking> bookingsRepository)
	{
		private readonly IRepository<Product> _productRepository = productRepository;
		private readonly IRepository<Booking> _bookingsRepository = bookingsRepository;

		public async Task<List<TimeSlotDTO>> GetSlotsAsync(int productId, DateTime date)
		{
			var product = await _productRepository.GetByIdAsync(productId);
			var allBookings = await _bookingsRepository.GetAllAsync();

			var bookings = allBookings
				.Where(b => b.ProductId == productId && b.BookingDate.Date == date.Date)
				.ToList();

			var result = new List<TimeSlotDTO>();

			for (var time = product.OpenTime; time < product.CloseTime; time.Add(TimeSpan.FromMinutes(product.IntervalMinutes)))
			{
				int count = bookings.Count(b => b.BookingDate.TimeOfDay == time);

				result.Add(new TimeSlotDTO(time, product.CapacityPerSlot, count));
			}

			return result;
		}

		public async Task<List<ResponseBookingDTO>> GetBookingsForProductAsync(int productId)
		{
			var bookings = await _bookingsRepository.GetAllAsync();
			var result = bookings
				.Where(b => b.ProductId == productId)
				.Select(b => new ResponseBookingDTO
				{
					Id = b.Id,
					BookingDate = b.BookingDate,
					ProductId = b.ProductId,
					CustomerName = b.CustomerName,
					CustomerEmail = b.CustomerEmail,
					CustomerPhone = b.CustomerPhone,
					Quantity = b.Quantity
				})
				.ToList();

			return result;
		}

		public async Task<List<ResponseBookingDTO>> GetBookingsByRangeAsync(int productId, DateTime start, DateTime end)
		{
			var allBookings = await _bookingsRepository.GetAllAsync();
			var bookings = allBookings.Where(b => b.ProductId == productId &&
												  b.BookingDate.Date >= start.Date &&
												  b.BookingDate.Date <= end.Date)
				.OrderBy(b => b.BookingDate)
				.ThenBy(b => b.BookingTime)
				.Select(b => new ResponseBookingDTO
				{
					Id = b.Id,
					BookingDate = b.BookingDate,
					ProductId = b.ProductId,
					CustomerName = b.CustomerName,
					CustomerEmail = b.CustomerEmail,
					CustomerPhone = b.CustomerPhone,
					Quantity = b.Quantity
				})
				.ToList();

			return bookings;
		}
	}
}