using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompositeKey.API.Attributes;
using CompositeKey.API.Base;
using CompositeKey.Domain.ViewModels;
using CompositeKey.Model;
using CompositeKey.Infrastructure;

namespace CompositeKey.API.Controllers
{
    public class AreasController : CommonController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly CompositeKeyContext _context;

        public AreasController(
            IConfiguration configuration,
            CompositeKeyContext context,
            IMapper mapper,
            ILogger<AreasController> logger)
            : base(configuration, mapper, logger)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all areas info
        /// </summary>
        /// <returns>Areas list</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AreaViewModel>>> GetAreas()
        {
            var areas = await _context.Areas
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .ToListAsync();

            var result = _mapper.Map<IEnumerable<AreaViewModel>>(areas);
            return Ok(result);
        }

        /// <summary>
        /// Get area info by id
        /// </summary>
        /// <returns>Area</returns>
        [HttpGet("{areaId}")]
        public async Task<ActionResult<IEnumerable<AreaViewModel>>> GetArea(string areaId)
        {
            var area = await _context.Areas
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync(x => x.Id == areaId);

            if (area == null)
                return NotExist(areaId);

            var result = _mapper.Map<AreaViewModel>(area);
            return Ok(result);
        }

        /// <summary>
        /// Creating new area info
        /// </summary>
        /// <returns>Action result</returns>
        [HttpPut("create")]
        [ValidateModelState]
        public async Task<ActionResult> AddArea([FromBody] AreaAddViewModel model)
        {
            var area = _mapper.Map<Area>(model);

            var user = await _context.Users
                .FirstOrDefaultAsync();

            if (user == null)
                return UserNotFound();

            area.CreatedById = user.Id;
            area.Id = area.Id;

            _context.Areas.Add(area);
            await _context.SaveChangesAsync();

            return OkCreateResponse(user, model.Name);
        }

        /// <summary>
        /// Editing area info
        /// </summary>
        /// <returns>Action result</returns>
        [HttpPut("edit")]
        [ValidateModelState]
        public async Task<ActionResult> EditArea([FromBody] AreaEditViewModel model)
        {
            var existingArea = await _context.Areas
                .FirstOrDefaultAsync(x => x.Id == model.Id);
            if (existingArea == null)
                return NotExist(model.Id);

            var area = _mapper.Map(model, existingArea);

            var user = await _context.Users
                .FirstOrDefaultAsync();
            if (user == null)
                return UserNotFound();

            area.UpdatedById = user.Id;
            area.UpdatedOn = DateTimeOffset.Now;

            _context.Areas.Update(area);
            await _context.SaveChangesAsync();

            return OkUpdateResponse(user, model.Id);
        }

        /// <summary>
        /// Deleting area info
        /// </summary>
        /// <returns>Action result</returns>
        [HttpDelete("{areaId}")]
        [ValidateModelState]
        public async Task<ActionResult> DeleteArea(string areaId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync();
            if (user == null)
                return UserNotFound();

            var existingArea = await _context.Areas
                .FirstOrDefaultAsync(x => x.Id == areaId);
            if (existingArea == null)
                return NotExist(areaId);

            _context.Areas.Remove(existingArea);
            await _context.SaveChangesAsync();

            return OkDeleteResponse(user, areaId);
        }

    }
}
