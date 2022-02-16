using System.ComponentModel.DataAnnotations;

namespace NewsFeed.Api.Models
{
    public class CreatePublicationModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public string AuthorId { get; set; }
    }
}