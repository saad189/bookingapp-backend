using bookingapp_backend.Configurations;
using bookingapp_backend.Helpers;
using bookingapp_backend.Models;
using bookingapp_backend.Repository;
using bookingapp_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace bookingapp_backend.Services.Implementations
{
    public class LoginService : ILoginService
    {

        public readonly DBContext dbContext;

        public readonly JwtSetting _jwtSetting;

        public readonly EncryptionKey _encryptionKey;
        public LoginService(DBContext context, IOptions<JwtSetting> options, IOptions<EncryptionKey> encryptionKey)
        {
            dbContext = context;
            _jwtSetting = options.Value;
            _encryptionKey = encryptionKey.Value;
        }

        public async Task<Instructor> CheckLogin(string Uid, string password)
        {
            string convertedPassword = Security.SecurePassword(password, _encryptionKey.Key);
            var instructorInDb = await dbContext.Instructors.FirstOrDefaultAsync(i => (i.Uid == Uid ||i.Email == Uid) && i.Password == convertedPassword);
            return instructorInDb ?? null;
        }

        public string GenerateJwtToken(Instructor instructor)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,instructor.Name),
                new Claim(ClaimTypes.Email,instructor.Email),
                new Claim(ClaimTypes.Role,instructor.Role.ToString()),

            };

            var token = new JwtSecurityToken(_jwtSetting.Issuer, _jwtSetting.Audience, claims, expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
