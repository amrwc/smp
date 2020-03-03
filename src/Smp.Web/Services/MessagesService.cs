using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smp.Web.Models;
using Smp.Web.Repositories;

namespace Smp.Web.Services
{
    public interface IMessagesService
    {
        Task CreateMessage(Message message);
        Task<IList<Message>> GetMessagesFromConversation(Guid conversationId, int count, int page);
    }

    public class MessagesService : IMessagesService
    {
        private readonly IMessagesRepository _messagesRepository;

        public MessagesService(IMessagesRepository messagesRepository)
        {
            _messagesRepository = messagesRepository;
        }

        public async Task CreateMessage(Message message)
            => await _messagesRepository.CreateMessage(message);

        public async Task<IList<Message>> GetMessagesFromConversation(Guid conversationId, int count, int page)
            => await _messagesRepository.GetMessagesByConversationId(conversationId, count, page, false);
    }
}