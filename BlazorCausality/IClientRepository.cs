using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorCausality
{
    public interface IClientRepository<TEntity> where TEntity : class
    {
        Task<bool> DeleteAsync(object id);
        Task<bool> DeleteAsync(TEntity entityToDelete);
        Task<List<TEntity>> GetAsync(string jsonQuery);
        Task<TEntity> GetByIdAsync(object id);
        Task<TEntity> InsertAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entityToUpdate);
    }
}