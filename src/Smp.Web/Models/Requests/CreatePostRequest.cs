using System;
using System.ComponentModel.DataAnnotations;

namespace Smp.Web.Models.Requests
{
    public class CreatePostRequest
    {
        [Required]
        public Guid ReceiverId { get; set; }
        [Required]
        public Guid SenderId { get; set; }
        [Required]
        public string Content { get; set; }
    }
}