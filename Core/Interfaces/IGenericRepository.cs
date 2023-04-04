using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync (string id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> GetEntityWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
        void Add (T entity);
        void Update (T entity);
        void Delete (T entity);
        void UpdateList(T[] entities);
        Task<IReadOnlyList<ProductCategory>> GetAllChildrenCategoriesAsync(string id);

    }
}