using Microsoft.AspNetCore.Mvc;
using NewsFeed.Domain;

namespace NewsFeed.Api.Controllers
{
    public class NewsFeedController : ControllerBase
    {
        private readonly IPublicationService _publicationService;

        public NewsFeedController(IPublicationService publicationService)
        {
            _publicationService = publicationService;
        }
    }
}