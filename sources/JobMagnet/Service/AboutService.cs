using AutoMapper;
using JobMagnet.AutoMapper;
using JobMagnet.Entities;
using JobMagnet.Models;
using JobMagnet.Repository.Interface;
using JobMagnet.Service.Interface;

namespace JobMagnet.Service
{
    public class AboutService : IAboutService
    {
        private readonly IAboutRepository<AboutEntity> _repository;

        public AboutService(IAboutRepository<AboutEntity> repository)
        {
            _repository = repository;
        }

        public async Task<AboutModel> Create(AboutCreateRequest aboutCreateRequest)
        {
            var aboutEntity = Mappers.MapAboutCreate(aboutCreateRequest);
            await _repository.CreateAsync(aboutEntity);
            var aboutModel = Mappers.MapAboutModel(aboutEntity);
            return aboutModel;
        }
        public async Task<AboutModel> GetById(int id)
        {
            var aboutEntity = await _repository.GetByIdAsync(id);
            var aboutModel = Mappers.MapAboutModel(aboutEntity);
            return aboutModel;
        }
    }
}
