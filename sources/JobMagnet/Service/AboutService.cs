using JobMagnet.AutoMapper;
using JobMagnet.Entities;
using JobMagnet.Models;
using JobMagnet.Repositories.Interface;
using JobMagnet.Service.Interface;

namespace JobMagnet.Service;

public class AboutService(
    IQueryRepository<AboutEntity> queryRepository,
    ICommandRepository<AboutEntity> commandRepository) : IAboutService
{
    public async Task<AboutModel> Create(AboutCreateRequest aboutCreateRequest)
    {
        var aboutEntity = Mappers.MapAboutCreate(aboutCreateRequest);
        await commandRepository.CreateAsync(aboutEntity);
        var aboutModel = Mappers.MapAboutModel(aboutEntity);
        return aboutModel;
    }

    public async Task<AboutModel> GetByIdAsync(int id)
    {
        var aboutEntity = await queryRepository.GetByIdAsync(id);

        if (aboutEntity is null)
            return null;

        var aboutModel = Mappers.MapAboutModel(aboutEntity);
        return aboutModel;
    }
}