using AutoMapper;
using JobMagnet.Entities;
using JobMagnet.Models;
using JobMagnet.Repository.Interface;
using JobMagnet.Service.Interface;

namespace JobMagnet.Service
{
    public class SkillService : ISkillService
    {
        private readonly IMapper mapper;
        private readonly ISkillRepository<SkillEntity> repository;

        public SkillService(IMapper mapper, ISkillRepository<SkillEntity> repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<SkillModel> Create(SkillCreateRequest skillCreateRequest)
        {
            var skillEntity = mapper.Map<SkillEntity>(skillCreateRequest);
            await repository.CreateAsync(skillEntity);

            var skillModel = mapper.Map<SkillModel>(skillEntity);
            return skillModel;
        }
    }
}
