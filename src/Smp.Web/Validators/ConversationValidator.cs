using System;
using System.Threading.Tasks;
using Smp.Web.Models.Requests;
using Smp.Web.Repositories;

namespace Smp.Web.Validators
{
    public interface IConversationValidator
    {
        Task<Guid> ValidateConversationDuplicate(CreateConversationRequest createConversationRequest);
    }

    public class ConversationValidator : IConversationValidator
    {
        private readonly IConversationsRepository _conversationsRepository;

        public ConversationValidator(IConversationsRepository conversationsRepository)
        {
            _conversationsRepository = conversationsRepository;
        }

        public async Task<Guid> ValidateConversationDuplicate(CreateConversationRequest createConversationRequest)
        {

            var commonConversationIds = await _conversationsRepository.GetCommonConversationIds(
                createConversationRequest.SenderId, createConversationRequest.ReceiverId);

            return commonConversationIds.Count > 0 ? commonConversationIds[0] : Guid.Empty;
        }
    }
}