namespace VedaVerk.Repositiories.Interfaces
{
	public interface IRepository <T> where T : class
	{
		Task<IEnumerable<T>> GetAllAsync();
		Task<T?> GetByIdAsync(int id);
		Task AddSync(T entity);
		Task UpdateAsync(T entity);
		Task DeleteAsync(int id);
	}
}
