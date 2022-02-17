using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsFeed.Api.Models;
using NewsFeed.Domain;

namespace NewsFeed.Api.Controllers
{
    public class NewsFeedController : ControllerBase
    {
        private readonly IPublicationService _publicationService;
        private readonly IUserProvider _userProvider;

        public NewsFeedController(IPublicationService publicationService, IUserProvider userProvider)
        {
            _publicationService = publicationService;
            _userProvider = userProvider;
        }
        
        /// <summary>
        /// Create one publication
        /// </summary>
        /// <param name="model">Publication</param>
        /// <returns>Created publication</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Publication>> CreateAsync([FromBody] CreatePublicationModel model)
        {
            var author = await _userProvider.GetByIdAsync(model.AuthorId);
            var id = await _publicationService.CreateAsync(model.Content, author);

            return Ok();
        }
        
        // [HttpGet]
        // public async Task<ActionResult<Publication>> SearchPublication
    }
}