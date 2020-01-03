using Smp.Web.Repositories;
using Smp.Web.Models;
using Smp.Web.Models.Requests;
using Smp.Web.Models.Results;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.Linq;

namespace Smp.Web.Services
{
    public interface IAuthService
    {
        Task<VerifyUserResult> VerifyUser(string email, string password);
        string CreateJwt(User user);
        bool AuthorizeSelf(string authToken, Guid userId);
        Task<bool> AuthorizeFriend(string authToken, Guid userId);
    }

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUsersRepository _userRepository;
        private readonly ICryptographyService _cryptographyService;
        private readonly IRelationshipsService _relationshipsService;

        public AuthService(IConfiguration configuration, IUsersRepository userRepository, ICryptographyService cryptographyService, IRelationshipsService relationshipsService)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _cryptographyService = cryptographyService;
            _relationshipsService = relationshipsService;
        }

        public async Task<VerifyUserResult> VerifyUser(string email, string password)
        {
            var userFromDb = await _userRepository.GetUserByEmail(email);
            if (userFromDb == null) return new VerifyUserResult(false, null);

            var isPasswordCorrect = _cryptographyService.CheckPassword(password, userFromDb.Password);

            return new VerifyUserResult(isPasswordCorrect, userFromDb);
        }

        public string CreateJwt(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.FullName),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Convert.FromBase64String(_configuration["Tokens:SigningKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Tokens:Issuer"],
                _configuration["Tokens:Issuer"],
                claims, expires: DateTime.Now.AddYears(10),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool AuthorizeSelf(string authToken, Guid userId) 
            => new JwtSecurityTokenHandler().ReadJwtToken(StripBearer(authToken)).Id == userId.ToString();

        public async Task<bool> AuthorizeFriend(string authToken, Guid userId)
        {
            var senderId = Guid.Parse(new JwtSecurityTokenHandler().ReadJwtToken(StripBearer(authToken)).Id);

            return await _relationshipsService.AreAlreadyFriends(senderId, userId);
        }

        private static string StripBearer(string authToken)
            => authToken.Substring(7);
    }
}