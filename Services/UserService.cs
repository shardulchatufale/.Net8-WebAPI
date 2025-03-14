﻿using DotNet8WebAPI.Entity;
using DotNet8WebAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotNet8WebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppSetting _appSettings;
        private readonly OurHeroDbContext db;


        public UserService(IOptions<AppSetting> appSettings, OurHeroDbContext _db)
        {
            _appSettings = appSettings.Value;
            db = _db;
        }

        public async Task<User?> AddAndUpdateUser(User userObj)
        {
            bool isSuccess = false;

            if (userObj.Id > 0)
            {
                var foundObj = await db.Users.FirstOrDefaultAsync(x => x.Id == userObj.Id);
                if (foundObj != null)
                {
                    foundObj.FirstName = userObj.FirstName;
                    foundObj.LastName = userObj.LastName;
                    db.Users.Update(foundObj); // Correctly updates the object
                    isSuccess = await db.SaveChangesAsync() > 0;
                    return isSuccess ? foundObj : null; // Return the updated object
                }
            }
            else
            {
                await db.Users.AddAsync(userObj); // Add new user if Id is zero
                isSuccess = await db.SaveChangesAsync() > 0;
            }

            return isSuccess ? userObj : null;
        }


        public async Task<AuthenticateResponse?> Authenticate(AuthenticationRequest model)
        {
            var FoundUser = await db.Users.SingleOrDefaultAsync(x => x.UserName == model.Username && x.Password == model.Password);
            if (FoundUser == null)  return null; 

            var token = await GenerateJwtToken(FoundUser);
            return new AuthenticateResponse(FoundUser, token);
        }

        public async Task<IEnumerable<User>> GetALL()
        {
            return await db.Users.Where(x => x.isActive == true).ToListAsync();
        }

        public async Task<User> GetByID(int id)
        {
            return await db.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await Task.Run(() =>
            {
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }), // Fixed syntax here
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                return tokenHandler.CreateToken(tokenDescriptor);
            });
            return tokenHandler.WriteToken(token);
        }

    }
}
