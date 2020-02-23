using System;
using System.ComponentModel.DataAnnotations;
using Smp.Web.Resources.Constants;

namespace Smp.Web.Models.Requests
{
    public class CreateMessageRequest
    {
        [Required]
        public Guid SenderId { get; set; }
        
        [Required, StringLength(10000, ErrorMessage = ErrorMessages.ValueTooLong)]
        public string Content { get; set; }

        [Required]
        public Guid ConversationId { get; set; }
    }
}