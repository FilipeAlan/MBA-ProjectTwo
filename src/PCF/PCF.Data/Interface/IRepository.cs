namespace PCF.Data.Interface
{
    public interface IRepository<T> where T:class
    {
        Task<T> CreateAsync(T entity);
        Task<T> ReadAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(object id);
    }
}
