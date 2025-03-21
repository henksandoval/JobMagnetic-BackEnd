using JobMagnet.AutoMapper;
using JobMagnet.Entities;
using JobMagnet.Models;
using JobMagnet.Repository.Interface;
using JobMagnet.Service.Interface;

namespace JobMagnet.Service;

public class SkillService(ISkillRepository<SkillEntity> repository) : ISkillService
{
    public async Task<SkillModel> Create(SkillCreateRequest skillCreateRequest)
    {
        var skillEntity = Mappers.MapSkillCreate(skillCreateRequest);
        await repository.CreateAsync(skillEntity);

        var skillModel = Mappers.MapSkillModel(skillEntity);
        return skillModel;
    }
}