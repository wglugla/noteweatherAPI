﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using LoggerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.Helpers;

namespace server.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;

        public EventsController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            try
            {
                var events = await _repository.Event.GetAllEventsAsync();
                _logger.LogInfo($"Returned all owners from database.");

                return Ok(events);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllOwners action: {ex.Message}");
                return StatusCode(500, "Internal server error" + ex);
            }
        }

        // GET: api/Events/5
        [HttpGet("{id}", Name =  "EventById")]
        public async Task<IActionResult> GetEventById(int id)
        {
            try
            {
                Event res = await _repository.Event.GetEventByIdAsync(id);
                int[] tagsId = await _repository.EventsTags.GetEventTagsAsync(res.Id);
                List<Tag> tags = new List<Tag>();
                foreach(int tagId in tagsId)
                {
                    Tag newTag = await _repository.Tag.GetTagByIdAsync(tagId);
                    tags.Add(newTag);
                }
                EventDetails details = new EventDetails(res, tags.Select(p => p.Value).ToArray());
                _logger.LogInfo($"Returned event by id from database.");

                return Ok(details);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetEventById action: {ex.Message}");
                return StatusCode(500, "Internal server error" + ex);
            }
        }

        //// POST: api/Events
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT: api/Events/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}