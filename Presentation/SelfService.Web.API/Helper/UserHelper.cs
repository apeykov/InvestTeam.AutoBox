using InvestTeam.AutoBox.Application.Common;
using InvestTeam.AutoBox.Domain.Enums;
using InvestTeam.AutoBox.SelfService.Web.API.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace InvestTeam.AutoBox.SelfService.Web.API.Helper
{
    public class UserHelper
    {
        private UserManager<User> userMng;
        private RoleManager<IdentityRole> roleMng;

        public UserHelper(UserManager<User> userMng, RoleManager<IdentityRole> roleMng)
        {
            this.userMng = userMng;
            this.roleMng = roleMng;
        }

        public async Task<OperationResult<User>> UpdateUser(UserDto userDTO)
        {
            OperationResult<User> result = new OperationResult<User>();
            User user = null;

            try
            {
                user = userMng.FindByIdAsync(userDTO.Identity).Result;
                if (user != null)
                {
                    if (!String.IsNullOrEmpty(userDTO.Email))
                    {
                        user.UserName = userDTO.Email;
                        user.Email = userDTO.Email;
                    }

                    if (!String.IsNullOrEmpty(userDTO.Name))
                    {
                        user.Name = userDTO.Name;
                    }

                    if (!String.IsNullOrEmpty(userDTO.Phone))
                    {
                        user.PhoneNumber = userDTO.Phone;
                    }

                    if (!String.IsNullOrEmpty(userDTO.Password))
                    {
                        await userMng.AddPasswordAsync(user, userDTO.Password);
                    }

                    await userMng.UpdateAsync(user);

                    result = new OperationResult<User>(user);
                }
            }
            catch (Exception ex)
            {
                if (user == null)
                {
                    user = new User
                    {
                        Id = userDTO.Identity,
                        Name = userDTO.Name,
                        Email = userDTO.Email,
                        PhoneNumber = userDTO.Phone
                    };
                }
                result.AddException(ex, user);
            }
            return await Task.FromResult(result);
        }

        public async Task<OperationResult<User>> CreateUser(UserDto userDTO)
        {
            OperationResult<User> result = new OperationResult<User>();
            User user = null;

            try
            {
                user = userMng.FindByEmailAsync(userDTO.Email).Result;
                if (user == null)
                {
                    user = new User
                    {
                        UserName = userDTO.Email,
                        Email = userDTO.Email,
                        Name = userDTO.Name,
                        PhoneNumber = userDTO.Phone,
                        EmailConfirmed = true
                    };
                    await userMng.CreateAsync(user);
                    user = await userMng.FindByEmailAsync(user.UserName);
                    await userMng.AddPasswordAsync(user, userDTO.Password);

                    string role = Enum.GetName(typeof(Role), userDTO.Role);
                    if (!await userMng.IsInRoleAsync(user, role))
                    {
                        await userMng.AddToRoleAsync(user, role);
                    }
                }

                result = new OperationResult<User>(user);
            }
            catch (Exception ex)
            {
                if (user == null)
                {
                    user = new User
                    {
                        Id = userDTO.Identity,
                        Name = userDTO.Name,
                        Email = userDTO.Email,
                        PhoneNumber = userDTO.Phone
                    };
                }
                result.AddException(ex, user);
            }
            return await Task.FromResult(result);
        }

        public async Task<OperationResult<UserDto>> GetUser(string identity)
        {
            OperationResult<UserDto> result = new OperationResult<UserDto>();
            UserDto userDto = null;
            try
            {
                User user = await userMng.FindByIdAsync(identity);
                if (user != null)
                {
                    result = new OperationResult<UserDto>(new UserDto
                    {
                        Identity = user.Id,
                        Email = user.Email,
                        Phone = user.PhoneNumber,
                        Name = user.Name
                    });
                }
            }
            catch (Exception ex)
            {
                if (userDto == null)
                {
                    userDto = new UserDto
                    {
                        Identity = identity
                    };
                }
                result.AddException(ex, userDto);
            }
            return result;
        }

        public async Task<OperationResult<User>> RemoveUser(string identity)
        {
            OperationResult<User> result;
            User user = null;
            try
            {
                user = await userMng.FindByIdAsync(identity);
                result = new OperationResult<User>(user);
                await userMng.DeleteAsync(user);
            }
            catch (Exception ex)
            {
                result = new OperationResult<User>();
                if (user == null)
                {
                    user = new User
                    {
                        Id = identity
                    };
                }
                result.AddException(ex, user);
            }
            return result;
        }
    }
}
