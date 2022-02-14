using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Profile.Domain;
using Swashbuckle.AspNetCore.Filters;

namespace Profiles.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RelationController : ControllerBase
    {
        private readonly IRelationsService _relationsService;

        public RelationController(IRelationsService relationsService)
        {
            _relationsService = relationsService;
        }
        
        /// <summary>
        /// Search user's followers
        /// </summary>
        /// <param name="user">Filters by user</param>
        /// <response code="200">Follower ids</response>
        [HttpGet("{user:guid}/followers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of followers")]
        public async Task<ActionResult<IEnumerable<Guid>>> GetAllFollowersAsync(
            [FromRoute] Guid user)
        {
            var followers = await _relationsService.GetAllFollowersAsync(user);

            return Ok(followers);
        }
        
        /// <summary>
        /// Search user's followers
        /// </summary>
        /// <param name="skip">Skip followers up to a specified position</param>
        /// <param name="take">Take followers up to a specified position</param>
        /// <param name="user">Filters by user</param>
        /// <response code="200">Follower ids</response>
        [HttpGet("{user:guid}/search/followers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-TotalCount", "Number", "Total number of followers")]
        public async Task<ActionResult<IEnumerable<Guid>>> SearchFollowersAsync(
            [FromQuery, Range(0, int.MaxValue)] int skip,
            [FromQuery, Range(1, 100)] int take,
            [FromRoute] Guid user)
        {
            var (followers, totalCount) = await _relationsService.SearchFollowersAsync(skip, take, user);
            Response.Headers.Add("X-TotalCount", totalCount.ToString());

            return Ok(followers);
        }
        
        /// <summary>
        /// Sent friend request
        /// </summary>
        /// <param name="fromUser">User who is sending request</param>
        /// <param name="toUser">User to whom the request was sent</param>
        /// <response code="204">Friend request successfully created</response>
        /// <response code="400"></response>
        [HttpPost("{fromUser:guid}/friends/{toUser:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SendFriendRequestAsync([FromRoute] Guid fromUser, [FromRoute] Guid toUser)
        {
            if (fromUser == toUser)
            {
                return BadRequest();
            }

            await _relationsService.SendRequestAsync(fromUser, toUser);

            return NoContent();
        }
    }
}