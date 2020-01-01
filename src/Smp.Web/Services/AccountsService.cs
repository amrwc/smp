using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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

        private readonly string _webAppUrl;

        public AccountsService(IActionsRepository actionsRepository, IUsersRepository usersRepository, IMailService mailService, IConfiguration configuration)
        {
            _actionsRepository = actionsRepository;
            _usersRepository = usersRepository;
            _mailService = mailService;
            _webAppUrl = configuration.GetValue<string>("WebApp:Url") ?? "https://localhost:5001/";
        }

        public async Task InitiatePasswordReset(Guid userId, string email)
        {
            var action = new Models.Action(userId, ActionType.ResetPassword);
            await _actionsRepository.CreateAction(action);
            await _mailService.SendEmail(email, "SMP - Password Reset", $"<h1>RESET PASSWORD</h1><p>CLICK THIS TO RESET PASSWORD <a>{_webAppUrl}reset-password/{action.Id}</a>.</p>");
        }

        public async Task CompletePasswordReset(Guid userId, string newPassword, Guid actionId)
            {
                await _usersRepository.UpdatePasswordById(userId, newPassword);
                await _actionsRepository.CompleteActionById(actionId);
            }
    }
}