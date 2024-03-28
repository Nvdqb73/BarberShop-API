using AutoMapper;
using AutoMapper.QueryableExtensions;
using BarberShop.Data;
using BarberShop.Entity;
using System.Linq.Expressions;

namespace BarberShop.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly BarberShopContext _context;
        private readonly IMapper _mapper;

        public GenericRepository(BarberShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public TDto GetById<TDto>(int id)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity == null)
            {
                throw new ArgumentException($"Không tìm thấy đối tượng với ID {id}.");
            }
            return MapToDto<TDto>(entity);
        }

        public IEnumerable<TDto> GetAll<TDto>()
        {
            var entities = _context.Set<T>().ToList();
            return entities.Select(entity => MapToDto<TDto>(entity));
        }

        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task UpdateProperties(int id, Action<T> updateAction)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                updateAction(entity);
            }
        }

        public async Task Delete(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
                _context.Set<T>().Remove(entity);
        }


        private TModel MapToDto<TModel>(T entity)
        {
            return _mapper.Map<TModel>(entity);
        }

        public IEnumerable<TDto> Search<TDto>(Expression<Func<T, bool>> predicate)
        {
            var entities = _context.Set<T>().Where(predicate).ToList();
            return entities.Select(entity => MapToDto<TDto>(entity));
        }

    }

}
