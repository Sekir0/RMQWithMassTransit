using System;
using System.Collections.Generic;

namespace NewsFeed.Domain
{
    public class Publication
    {
        public Publication(string id, string content, UserInfo author, DateTimeOffset createdOn)
        {
            Id = id;
            Content = content;
            Author = author;
            CreatedOn = createdOn;
        }

        public string Id { get; }

        public string Content { get; private set; }

        public UserInfo Author { get; }

        public DateTimeOffset CreatedOn { get; }

        public static Publication New(string content, UserInfo author)
        {
            var now = DateTimeOffset.UtcNow;

            return new Publication(default, content, author, now);
        }
    }
}