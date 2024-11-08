using AutoMapper;
using JobMagnet.AutoMapper;
using JobMagnet.Entities;
using JobMagnet.Models;
using JobMagnet.Repository.Interface;
using JobMagnet.Service.Interface;

namespace JobMagnet.Service
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository<SkillEntity> _repository;

        public SkillService(ISkillRepository<SkillEntity> repository)
        {
            _repository = repository;
        }
        public async Task<SkillModel> Create(SkillCreateRequest skillCreateRequest)
        {
            var skillEntity = Mappers.MapSkillCreate(skillCreateRequest);
            await _repository.CreateAsync(skillEntity);

            var skillModel = Mappers.MapSkillModel(skillEntity);
            return skillModel;
        }
    }
}
