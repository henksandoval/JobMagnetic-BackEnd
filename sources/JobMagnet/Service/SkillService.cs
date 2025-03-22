using JobMagnet.AutoMapper;
using JobMagnet.Entities;
using JobMagnet.Models;
using JobMagnet.Repositories.Interface;
using JobMagnet.Service.Interface;

namespace JobMagnet.Service;

public class SkillService(ICommandRepository<SkillEntity> commandRepository) : ISkillService
{
    public async Task<SkillModel> Create(SkillCreateRequest skillCreateRequest)
    {
        var skillEntity = Mappers.MapSkillCreate(skillCreateRequest);
        await commandRepository.CreateAsync(skillEntity);

        var skillModel = Mappers.MapSkillModel(skillEntity);
        return skillModel;
    }
}