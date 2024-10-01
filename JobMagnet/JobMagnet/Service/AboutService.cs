﻿using AutoMapper;
using JobMagnet.Entities;
using JobMagnet.Models;
using JobMagnet.Repository;

namespace JobMagnet.Service
{
    public class AboutService : IAboutService
    {
        private readonly Mapper mapper;
        private readonly IRepository<AboutEntity> repository;

        public AboutService(Mapper mapper, IRepository<AboutEntity> repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<AboutModel> Create(AboutCreateRequest aboutCreateRequest)
        {
            var aboutEntity = mapper.Map<AboutEntity>(aboutCreateRequest);
            await repository.CreateAsync(aboutEntity);
            var aboutModel = mapper.Map<AboutModel>(aboutEntity);
            return aboutModel;
        }

        public async Task<AboutModel> GetById(int id)
        {
            var aboutEntity = await repository.GetByIdAsync(id);
            var aboutModel = mapper.Map<AboutModel>(aboutEntity);
            return aboutModel;
        }
    }
}
