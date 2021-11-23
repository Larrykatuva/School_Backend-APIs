using Microsoft.AspNetCore.Identity;
using SchoolBackendAPIs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolBackendAPIs.Data.Services
{
    public interface IUserRepository
    {
        public Task<ApplicationUser> FindUserById(string userId);
        public Task<ApplicationUser> FindUserByEmail(string email);
        public Task<ApplicationUser> FindUserByName(string username);
        public Task<IdentityResult> CreateUser(ApplicationUser user, string password);
        public Task CreateUserRole(string role);
        public Task AssignUserRole(ApplicationUser user,string role);
        public Task<bool> CheckIfRoleExist(string role);
        public Task<LoginResponse> ClaimUserRoles(ApplicationUser user);
        public Task<bool> LoginUser(ApplicationUser user, string password);
        public Task<string> GetEmailConfirmationToken(ApplicationUser user);
        public Task<IdentityResult> ConfirmEmail(ApplicationUser user, string token);
        public Task<string> GetPasswordResetToken(ApplicationUser user);
        public Task<IdentityResult> UpdateUserPassword(ApplicationUser user, string token, string password);
    }
}
