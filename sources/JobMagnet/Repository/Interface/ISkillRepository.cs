namespace JobMagnet.Repository.Interface
{
    public interface ISkillRepository<TEntity> where TEntity : class
    {
        Task CreateAsync(TEntity entity);
    }
}
