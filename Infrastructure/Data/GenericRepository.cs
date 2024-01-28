using System.Reflection;
using Core.Entities;
using Core.Entities.Comparers;
using Core.Entities.PriceListAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;
        public GenericRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }


        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            if (typeof(T) == typeof(Product))
            {
                var productsWithPrices = await _context.Set<Product>()
                    .Include(p => p.Prices)
                        .ThenInclude(x => x.PriceType)
                        .ThenInclude(pt => pt.Currency)
                    .ToListAsync();

                // Convert the list of Product entities to a list of T entities
                var entities = productsWithPrices.Cast<T>().ToList();

                // Find the price with the maximum date for each price type ID for each product
                foreach (var entity in entities)
                {
                    var product = (Product)(object)entity;
                    foreach (var priceTypeGroup in product.Prices.GroupBy(p => p.PriceTypeId))
                    {
                        // var maxDatePrice = priceTypeGroup.OrderByDescending(p => p.DateTime).FirstOrDefault();
                        var maxDatePrices = product.Prices
                            .GroupBy(p => p.PriceTypeId)
                            .Select(g => g.OrderByDescending(p => p.DateTime).FirstOrDefault())
                            .ToList();
                        product.Prices = maxDatePrices;
                    }
                }

                return entities;

            }

            if (typeof(T) == typeof(PriceType))
            {
                var priceTypes = await _context.Set<PriceType>()
                    .Include(p => p.Currency)
                    .Cast<T>()
                    .ToListAsync();

                return priceTypes;    
            }

            return await _context.Set<T>().ToListAsync();
        }


        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            var query = ApplySpecification(spec);

            if (typeof(T) == typeof(Product))
            {
                query = query.Include(p => (p as Product).Prices)
                             .ThenInclude(p => p.PriceType)
                             .ThenInclude(pt => pt.Currency);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            var query = ApplySpecification(spec);

            if (typeof(T) == typeof(Product))
            {
                query = query.Include(p => (p as Product).Prices)
                                .ThenInclude(p => p.PriceType)
                                .ThenInclude(pt => pt.Currency);
            }

            return await query.ToListAsync();

        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {

            return await ApplySpecification(spec).CountAsync();

        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<IReadOnlyList<ProductCategory>> GetAllChildrenCategoriesAsync(string id)
        {
            var allCategories = await _context.Set<ProductCategory>().ToListAsync();

            if (string.IsNullOrEmpty(id))
            {
                return allCategories;
            }

            if (allCategories == null || allCategories.FirstOrDefault(c => c.Id == id) == null)
            {
                return null;
            }

            var children = new List<ProductCategory> { allCategories.FirstOrDefault(c => c.Id == id) }; // add the main category to the list

            Action<string> getChildren = null;
            getChildren = (id) =>
            {
                var subChildren = allCategories.Where(c => c.ParentId == id);
                if (subChildren != null && subChildren.Any())
                {
                    children.AddRange(subChildren);
                    foreach (var child in subChildren)
                    {
                        getChildren(child.Id);
                    }
                }
            };

            getChildren(id);

            return children;
        }

        public async void UpdateList(T[] entities)
        {
            var entitiesInDb = await this.ListAllAsync();
            foreach (var entity in entities)
            {
                T entityInDb = entitiesInDb.FirstOrDefault(e => e.Id == entity.Id) ?? null;
                if (entityInDb is null)
                {
                    // create new Entity and save it to DB
                    this.Add(entity);
                }
                else
                {
                    var comparerGeneric = new CompareEntities<T>();
                    if (comparerGeneric.Equals(entity, entityInDb))
                    {
                        // don't update, Entity not changed
                    }
                    else
                    {
                        // Entity changed, update
                        var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                        foreach (var property in properties)
                        {
                            if (property.GetValue(entity) != null)
                            {
                                property.SetValue(entityInDb, property.GetValue(entity));
                            }
                            else
                            {
                                property.SetValue(entityInDb, null);
                            }
                        }

                        this.Update(entityInDb);

                    }
                }
            }
        }


    }
}