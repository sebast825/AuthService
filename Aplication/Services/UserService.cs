using Core.Constants;
using Core.Dto.Auth;
using Core.Dto.User;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public class UserService : UserServicesI
    {
        private readonly UserRepositoryI _userRepositoryI;
        public UserService(UserRepositoryI userRepositoryI)
        {

            _userRepositoryI = userRepositoryI;
        }

        public async Task AddAsync(UserCreateRequestDto userCreateDto)
        {
            User user = new User { Email = userCreateDto.Email, Password = userCreateDto.Password, FullName = userCreateDto.FullName };
            user.Validate();
            //add hashpassword after validate, if not validation don't work on password
            user.Password = this.HashPassword(userCreateDto.Password);

            if (await this.EmailAlreadyUsed(user.Email))
            {
                throw new InvalidOperationException(ErrorMessages.EmailNotAviable);
            }

            await _userRepositoryI.AddAsync(user);
        }

        public async Task ValidateCredentialsAsync(LoginRequestDto loginDto)
        {
            User? user = await _userRepositoryI.GetByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new InvalidCredentialException(ErrorMessages.InvalidCredentials);

            }
            bool matchMassword = this.MatchPassword(loginDto.Password, user.Password);
            if (!matchMassword)
            {
                throw new InvalidCredentialException(ErrorMessages.InvalidCredentials);

            }
        }
        private async Task<bool> EmailAlreadyUsed(string email)
        {
            return await _userRepositoryI.GetByEmailAsync(email) != null;

        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        private bool MatchPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }

}
