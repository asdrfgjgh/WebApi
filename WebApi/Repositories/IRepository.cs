namespace WebApi.Repositories
{
    public interface IRepository
    {
        Task<IEnumerable<WebApi>> GetAllAsync();
        Task<WebApi> GetByWebApiIdAsync(Guid id);
        Task<IEnumerable<WebApi>> GetByUserIdAsync(string id);
        Task<WebApi> AddAsync(WebApi webApi); // Updated to return Task<WebApi>
        Task UpdateAsync(WebApi webApi);
        Task DeleteAsync(Guid id);
    }
}
