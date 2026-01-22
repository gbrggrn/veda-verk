using VedaVerk.Models;
using VedaVerk.Repositiories.Interfaces;

namespace VedaVerk.Repositiories.Implementations
{
	public class PizzaRepository : IRepository<Pizza>
	{
		public Task AddSync(Pizza entity)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Pizza>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Pizza?> GetByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(Pizza entity)
		{
			throw new NotImplementedException();
		}
	}
}
