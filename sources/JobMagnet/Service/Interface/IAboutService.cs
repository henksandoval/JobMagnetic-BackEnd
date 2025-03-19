using JobMagnet.Models;

namespace JobMagnet.Service.Interface;

public interface IAboutService
{
    public Task<AboutModel> GetById(int id);
    public Task<AboutModel> Create(AboutCreateRequest aboutCreateRequest);
}