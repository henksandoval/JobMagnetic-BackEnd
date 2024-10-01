﻿using AutoMapper;
using JobMagnet.Entities;
using JobMagnet.Models;
using JobMagnet.Repository;
using JobMagnet.Service;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class AboutController : ControllerBase
    {
        private readonly IAboutService service;

        public AboutController(IAboutService service)
        {
            this.service = service;
        }

        [HttpGet("{id}", Name = "GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var aboutModel = await service.GetById(id);

            if (aboutModel is null) 
            {
                return NotFound($"Record [{id}] not found");
            }

            return Ok(aboutModel);
        }

        //[HttpGet("GetTrue", Name = "GetTrue")]
        //public async Task<IActionResult> GetOk()
        //{
        //    var aboutEntity = new AboutEntity();
        //    return Ok(aboutEntity);
        //}

        [HttpPost]
        public async Task<ActionResult<AboutModel>> CreateAbout([FromBody] AboutCreateRequest aboutCreateRequest)
        {
            var aboutModel = await service.Create(aboutCreateRequest);
            return CreatedAtRoute("GetById", new { aboutModel.Id });
        }
    }
}
