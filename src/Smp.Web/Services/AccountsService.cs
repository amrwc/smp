using System;
using System.Threading.Tasks;
using Smp.Web.Models;
using Smp.Web.Repositories;

namespace Smp.Web.Services
{
    public interface IAccountsService
    {
        Task InitiatePasswordReset(Guid userId, string email);
    }

    public class AccountsService : IAccountsService
    {
        private readonly IActionsRepository _actionsRepository;
        private readonly IMailService _mailService;

        public AccountsService(IActionsRepository actionsRepository, IMailService mailService)
        {
            _actionsRepository = actionsRepository;
            _mailService = mailService;
        }

        public async Task InitiatePasswordReset(Guid userId, string email)
        {
            var action = new Models.Action(userId, ActionType.ResetPassword);
            await _actionsRepository.CreateAction(action);
            await _mailService.SendEmail(email, "", ""); //BLAH BLAH. CLICK LINK TO HIT THE NEXT API ENDPOINT WITH action.Id
        }
    }
}