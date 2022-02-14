using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Profile.Domain;
using Profiles.Api.Models;

namespace Profiles.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }
        
        /// <summary>
        /// Get profile by id
        /// </summary>
        /// <param name="id">Profile id</param>
        /// <response code="200">Returns profile</response>
        /// <response code="404">Profile not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Profile.Domain.Profile>> GetByIdAsync([FromRoute] Guid id)
        {
            var profile = await _profileService.GetByIdAsync(id);

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }
        
        /// <summary>
        /// Create one profile
        /// </summary>
        /// <param name="createModel">Profile</param>
        /// <response code="201">Profile successfully created</response>
        /// <response code="400">Validation failed</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateAsync([FromBody] ProfileCreateViewModel createModel)
        {
            var profile = await _profileService.CreateAsync(createModel.Id, createModel.FirstName, createModel.LastName, createModel.Gender, createModel.DateOfBirth, createModel.City);

            if (profile is not null)
            {
                return Created(Url.Action("GetById", new { profile.Id }), profile);
            }

            return BadRequest("Smt went wrong");
        }
    }
}