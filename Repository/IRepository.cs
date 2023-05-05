namespace Store_Core7.Repository
{
    public interface IRepository<TModel>
    {
        Task<TModel> FindByIdAsync(long id);
        Task<List<TModel>> FindAllAsync(int pageIndex, int pageSize);
        Task<long> AddAsync(TModel entity);
        Task<bool> UpdateAsync(TModel entity);
        Task<bool> DeleteAsync(long id);
    }
}
