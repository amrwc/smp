using System;
using System.Net.Http;
using System.Threading.Tasks;
using Smp.Web.Models;
using Smp.Web.Repositories;

namespace Smp.Web.Services
{
    public interface IAccountsService
    {
        Task InitiatePasswordReset(Guid userId, string email);
        Task CompletePasswordReset(Guid userId, string newPassword, Guid actionId);
    }

    public class AccountsService : IAccountsService
    {
        private readonly IActionsRepository _actionsRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IMailService _mailService;

        public AccountsService(IActionsRepository actionsRepository, IUsersRepository usersRepository, IMailService mailService)
        {
            _actionsRepository = actionsRepository;
            _usersRepository = usersRepository;
            _mailService = mailService;
        }

        public async Task InitiatePasswordReset(Guid userId, string email)
        {
            var action = new Models.Action(userId, ActionType.ResetPassword);
            await _actionsRepository.CreateAction(action);
            await _mailService.SendEmail(email, "Password reset", $"<h1>RESET PASSWORD</h1><p>CLICK THIS TO RESET PASSWORD<a>http://localhost:5001/resetpassword/{action.Id}</a>.");
            //BLAH BLAH. CLICK LINK TO GO TO WEB APP WHICH WILL HIT THE NEXT API ENDPOINT WITH action.Id
        }

        public async Task CompletePasswordReset(Guid userId, string newPassword, Guid actionId)
            {
                await _usersRepository.UpdatePasswordById(userId, newPassword);
                await _actionsRepository.CompleteActionById(actionId);
            }
    }
}