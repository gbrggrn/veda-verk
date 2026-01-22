using VedaVerk.Models;
using VedaVerk.Repositiories.Interfaces;

namespace VedaVerk.Repositiories.Implementations
{
	public class CourseRepository : IRepository<Course>
	{
		public Task AddSync(Course entity)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Course>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Course?> GetByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(Course entity)
		{
			throw new NotImplementedException();
		}
	}
}
