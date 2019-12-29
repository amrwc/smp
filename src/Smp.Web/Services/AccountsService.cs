using System;
using System.Threading.Tasks;
using Smp.Web.Models;

namespace Smp.Web.Services
{
    public interface IAccountsService
    {
        Task InitiateResetPassword(Guid userId, string email);
    }

    public class AccountsService : IAccountsService
    {
        private readonly IMailService _mailService;

        public AccountsService(IMailService mailService)
        {
            _mailService = mailService;
        }

        public async Task InitiateResetPassword(Guid userId, string email)
        {
            var action = new Models.Action(userId, ActionType.ResetPassword);
            //await _actionsRepository.CreateAction(action);
            await _mailService.SendEmail(email, "", ""); //BLAH BLAH. CLICK LINK TO HIT THE NEXT API ENDPOINT WITH action.Id
        }
    }
}