namespace ITLANetworking.Core.Application.Interfaces.Services.Common
{
    public interface IGenericService<SaveDto, Dto, Entity>
        where SaveDto : class
        where Dto : class
        where Entity : class
    {
        Task<SaveDto> Add(SaveDto dto);
        Task Update(SaveDto dto, int id);
        Task Delete(int id);

        Task<SaveDto?> GetByIdAsync(int id);
        Task<Dto?> GetDtoByIdAsync(int id);

        Task<List<Dto>> GetAllAsync();
        Task<List<Dto>> GetAllWithIncludeAsync(List<string> properties);

        Task<SaveDto?> GetByIdWithIncludeAsync(int id, List<string> properties);
        Task<List<Dto>> FindAsync(Func<Entity, bool> predicate);
    }
}
