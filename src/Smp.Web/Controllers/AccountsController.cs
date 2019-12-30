using System.Threading.Tasks;
using System.Linq;
using Smp.Web.Models;
using Smp.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Smp.Web.Services;
using Smp.Web.Models.Requests;
using Smp.Web.Validators;

namespace Smp.Web.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IActionsRepository _actionsRepository;
        private readonly IAccountsService _accountsService;
        private readonly IActionValidator _actionValidator;
        private readonly ICryptographyService _cryptographyService;

        public AccountsController(IUsersRepository usersRepository, IActionsRepository actionsRepository, IAccountsService accountsService, IActionValidator actionValidator, ICryptographyService cryptographyService)
        {
            _usersRepository = usersRepository;
            _actionsRepository = actionsRepository;
            _accountsService = accountsService;
            _actionValidator = actionValidator;
            _cryptographyService = cryptographyService;
        }

        [HttpGet("[action]/{email}")]
        public async Task<IActionResult> ForgottenPassword(string email)
        {
            var user = await _usersRepository.GetUserByEmail(email);
            if (user == null) return NotFound(new Error("invalid_email", "Email must be linked to an existing user account."));

            await _accountsService.InitiatePasswordReset(user.Id, user.Email);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordRequest resetPasswordRequest)
        {
            var action = await _actionsRepository.GetActionById(resetPasswordRequest.ActionId);
            var validationErrors = _actionValidator.ValidateAction(action, ActionType.ResetPassword);
            if (resetPasswordRequest.NewPassword != resetPasswordRequest.ConfirmNewPassword) validationErrors.Add(new Error("invalid_password", "Passwords must match."));
            if (validationErrors.Any()) return BadRequest(validationErrors);

            await _accountsService.CompletePasswordReset(action.UserId, _cryptographyService.HashAndSaltPassword(resetPasswordRequest.NewPassword), action.Id);

            return Ok();
        }
    }
}