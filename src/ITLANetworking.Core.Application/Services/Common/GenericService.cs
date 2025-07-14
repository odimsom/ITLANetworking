using AutoMapper;
using ITLANetworking.Core.Application.Interfaces.Services.Common;
using ITLANetworking.Core.Domain.Interfaces.Repositories;

namespace ITLANetworking.Core.Application.Services.Common
{
    public class GenericService<SaveDto, Dto, Entity> : IGenericService<SaveDto, Dto, Entity>
        where SaveDto : class
        where Dto : class
        where Entity : class
    {
        protected readonly IGenericRepository<Entity> _repository;
        protected readonly IMapper _mapper;

        public GenericService(IGenericRepository<Entity> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public virtual async Task<SaveDto> Add(SaveDto dto)
        {
            var entity = _mapper.Map<Entity>(dto);
            var addedEntity = await _repository.AddAsync(entity);
            return _mapper.Map<SaveDto>(addedEntity);
        }

        public virtual async Task Delete(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                await _repository.DeleteAsync(entity);
            }
        }

        public virtual async Task<List<Dto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<Dto>>(entities);
        }

        public virtual async Task<List<Dto>> GetAllWithIncludeAsync(List<string> properties)
        {
            var entities = await _repository.GetAllWithIncludeAsync(properties);
            return _mapper.Map<List<Dto>>(entities);
        }

        public virtual async Task<SaveDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<SaveDto>(entity);
        }

        public virtual async Task<Dto?> GetDtoByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<Dto>(entity);
        }

        public virtual async Task<SaveDto?> GetByIdWithIncludeAsync(int id, List<string> properties)
        {
            var entity = await _repository.GetByIdWithIncludeAsync(id, properties);
            return entity == null ? null : _mapper.Map<SaveDto>(entity);
        }

        public virtual async Task Update(SaveDto dto, int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                _mapper.Map(dto, entity);
                await _repository.UpdateAsync(entity, id);
            }
        }

        public virtual async Task<List<Dto>> FindAsync(Func<Entity, bool> predicate)
        {
            var entities = await _repository.FindAsync(e => predicate(e));
            return _mapper.Map<List<Dto>>(entities);
        }
    }
}
