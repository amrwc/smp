using System;
using Smp.Web.Models.Requests;

namespace Smp.Web.Models
{
    public class Post
    {
        public Post() {}

        public Post(CreatePostRequest post)
        {
            Id = Guid.NewGuid();
            ReceiverId = post.ReceiverId;
            SenderId = post.SenderId;
            Content = post.Content;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public static explicit operator Post(DTOs.Post post)
        {
            return new Post
            {
                Id = post.Id,
                ReceiverId = post.ReceiverId,
                SenderId = post.SenderId,
                Content = post.Content,
                CreatedAt = post.CreatedAt
            };
        }
    }
}