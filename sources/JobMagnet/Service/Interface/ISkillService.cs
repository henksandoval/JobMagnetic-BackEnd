using JobMagnet.Models;

namespace JobMagnet.Service.Interface;

public interface ISkillService
{
    public Task<SkillModel> Create(SkillCreateRequest skillCreateRequest);
}