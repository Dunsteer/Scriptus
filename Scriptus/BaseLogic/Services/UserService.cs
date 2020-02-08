using BaseLogic.Abstractions;
using BaseLogic.Helpers;
using Commons.Core;
using Commons.Models.User;
using DbServices.DataProviders;
using DbServices.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BaseLogic.Services
{
    public class UserService : BaseService<User>
    {
        public UserService(IDataProvider<User> db, MapperService mapper) : base(db, mapper)
        {
        }

        public override Task<User> Create(User model)
        {
            if (String.IsNullOrEmpty(model.Password)) return null;

            model.Password = Cryptography.GetSHA256Hash(model.Password);

            return base.Create(model);
        }

        public async Task<UserLoginResponseModel> Authenticate(string username, string password, string secret, int validFor = 7)
        {
            var userDB = _database as UserDB;

            var user = await userDB.CanLogin(username, Cryptography.GetSHA256Hash(password));
            if (user == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("username", user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(validFor),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var response = _mapper.Get().Map<UserLoginResponseModel>(user);
            response.Token = tokenHandler.WriteToken(token);

            return response;
        }

        public async Task<UserLoginResponseModel> AuthenticateExternal(Guid id, string fullname, string email, string secret, int validFor = 7)
        {
            var userDB = _database as UserDB;

            var user = await userDB.ReadOne(id);

            if (user == null)
            {
                user = await userDB.CreateOne(new User
                {
                    Email = email,
                    FullName = fullname,
                    Id = id,
                    Password = "",
                    Rank = 0,
                    Reputation = 0,
                    Username = email.Split("@")[0]
                });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("username", user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(validFor),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var response = _mapper.Get().Map<UserLoginResponseModel>(user);
            response.Token = tokenHandler.WriteToken(token);

            return response;
        }

        public async Task<User> GetByMail(string mail)
        {
            if (mail == null) return null;

            return (await _database.ReadMany(new UserSearchModel() { Email = mail })).FirstOrDefault();
        }

        public async Task<User> ResetPassword(User user, string oldPassword, string newPassword, string confirmPassword)
        {
            if (user == null) return null;

            if (string.IsNullOrWhiteSpace(oldPassword) || user.Password.ToLower() == Cryptography.GetSHA256Hash(oldPassword.Trim()).ToLower())
            {
                if (newPassword.Trim() == confirmPassword.Trim())
                {
                    user.Password = Cryptography.GetSHA256Hash(newPassword);

                    await _database.UpdateOne(user.Id, user);
                    return user;
                }
            }

            return null;
        }
    }
}
