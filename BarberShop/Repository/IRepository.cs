using System.Linq.Expressions;

namespace BarberShop.Repository
{
    public interface IRepository<T> where T : class
    {
        //T GetById(int id);
        TDto GetById<TDto>(int id);
        public Task Add(T entity);
        Task UpdateProperties(int id, Action<T> updateAction);
        public Task Delete(int id);
        IEnumerable<TDto> GetAll<TDto>();
        IEnumerable<TDto> Search<TDto>(Expression<Func<T, bool>> predicate);
    }
}
