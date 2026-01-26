using VedaVerk.Shared.DTOs;

namespace VedaVerk.Client.Services.Interfaces
{
	public interface IBookingService
	{
		Task<bool> Create(CreateBookingDTO dto);
		Task<bool> Cancel(int id, Guid token);
		Task<bool> Delete(int id);
		Task<List<TimeSlotDTO>> GetAvailableSlotsAsync(int productId, DateTime date);
		Task<List<ResponseBookingDTO>> GetBookingsForProduct(int productId);
		Task<List<ResponseBookingDTO>> GetBookingsByRange(int productId, DateTime start, DateTime end);
	}
}
