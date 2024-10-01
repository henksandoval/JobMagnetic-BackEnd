namespace JobMagnet.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task CreateAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(int id);
    }
}
