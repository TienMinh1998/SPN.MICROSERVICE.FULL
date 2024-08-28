
namespace SPNApplication.Services
{
    public interface IDapperService
    {
        Task Execute(string query, object parameters = null);
        Task<IEnumerable<T>> GetAllAsync<T>(string query, object parameters = null);
        T QueryFirst<T>(string query, object parameters = null);
        T QueryFirstOrDefault<T>(string query, object parameters = null);
        T QuerySingle<T>(string query, object parameters = null);
        T QuerySingleOrDefault<T>(string query, object parameters = null);
    }
}