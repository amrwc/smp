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

namespace Smp.Web.Services
{
    public interface IAuthService
    {
        Task<VerifyUserResult> VerifyUser(string email, string password);
        string CreateJwt(User user);
    }

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly ICryptographyService _cryptographyService;

        public AuthService(IConfiguration configuration, IUserRepository userRepository, ICryptographyService cryptographyService)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _cryptographyService = cryptographyService;
        }

        public async Task<VerifyUserResult> VerifyUser(string email, string password)
        {
            var userFromDb = await _userRepository.GetUser(email);

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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:SigningKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Tokens:Issuer"],
                audience: _configuration["Tokens:Issuer"],
                claims: claims, expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}