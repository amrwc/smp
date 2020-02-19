using System;
using System.ComponentModel.DataAnnotations;
using Smp.Web.Resources.Constants;

namespace Smp.Web.Models.Requests
{
    public class CreatePostRequest
    {
        [Required]
        public Guid ReceiverId { get; set; }

        [Required]
        public Guid SenderId { get; set; }

        [Required, StringLength(10000, ErrorMessage = ErrorMessages.ValueTooLong)]
        public string Content { get; set; }
    }
}
