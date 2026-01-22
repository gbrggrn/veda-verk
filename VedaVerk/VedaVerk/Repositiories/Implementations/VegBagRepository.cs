using VedaVerk.Models;
using VedaVerk.Repositiories.Interfaces;

namespace VedaVerk.Repositiories.Implementations
{
	public class VegBagRepository : IRepository<VegBag>
	{
		public Task AddSync(VegBag entity)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<VegBag>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<VegBag?> GetByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(VegBag entity)
		{
			throw new NotImplementedException();
		}
	}
}
