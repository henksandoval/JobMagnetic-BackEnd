using JobMagnet.AutoMapper;
using JobMagnet.Entities;
using JobMagnet.Models;
using JobMagnet.Repository.Interface;
using JobMagnet.Service.Interface;

namespace JobMagnet.Service;

public class AboutService(IAboutRepository<AboutEntity> repository) : IAboutService
{
    public async Task<AboutModel> Create(AboutCreateRequest aboutCreateRequest)
    {
        var aboutEntity = Mappers.MapAboutCreate(aboutCreateRequest);
        await repository.CreateAsync(aboutEntity);
        var aboutModel = Mappers.MapAboutModel(aboutEntity);
        return aboutModel;
    }

    public async Task<AboutModel> GetById(int id)
    {
        var aboutEntity = await repository.GetByIdAsync(id);

        if (aboutEntity is null)
            return null;

        var aboutModel = Mappers.MapAboutModel(aboutEntity);
        return aboutModel;
    }
}