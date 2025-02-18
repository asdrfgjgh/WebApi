namespace WebApi.Repositories
{
    public interface IRepository
    {
        Task<IEnumerable<WebApi>> GetAllAsync();
        Task<WebApi> GetByIdAsync(Guid id);
        Task<WebApi> AddAsync(WebApi webApi); // Updated to return Task<WebApi>
        Task UpdateAsync(WebApi webApi);
        Task DeleteAsync(Guid id);
    }
}
