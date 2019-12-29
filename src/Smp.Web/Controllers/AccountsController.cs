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
        private readonly IAccountsService _accountsService;
        private readonly IActionValidator _actionValidator;
        private readonly ICryptographyService _cryptographyService;

        public AccountsController(IUsersRepository usersRepository, IAccountsService accountsService, IActionValidator actionValidator, ICryptographyService cryptographyService)
        {
            _usersRepository = usersRepository;
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
            var validationErrors = await _actionValidator.ValidateAction(resetPasswordRequest.ActionId, ActionType.ResetPassword);
            if (resetPasswordRequest.NewPassword != resetPasswordRequest.ConfirmNewPassword) validationErrors.Add(new Error("invalid_password", "Passwords must match."));
            if (validationErrors.Any()) return BadRequest(validationErrors);

            await _accountsService.CompletePasswordReset(resetPasswordRequest.UserId, _cryptographyService.HashAndSaltPassword(resetPasswordRequest.NewPassword), resetPasswordRequest.ActionId);

            return Ok();
        }
    }
}