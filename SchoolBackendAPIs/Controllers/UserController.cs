using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SchoolBackendAPIs.Data.Models;
using SchoolBackendAPIs.Data.Services;
using SchoolBackendAPIs.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SchoolBackendAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly IConfiguration _configuration;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="emailRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="configuration"></param>
        public UserController(IUserRepository userRepository, IEmailRepository emailRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _emailRepository = emailRepository;
            _configuration = configuration;
        }


        /// <summary>
        /// Handles incoming request to create a new student
        /// </summary>
        /// <param name="studentVM"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register-student")]
        public async Task<IActionResult> RegisterStudent([FromBody] RegisterVM studentVM)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest,ModelState);
            }
            ApplicationUser exists = await _userRepository.FindUserByEmail(studentVM.FirstName);
            if (exists != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new ErrorResponse
                    {
                        Error = true,
                        Message = "Email already registered!",
                        Field = "Email"
                    });
            }
            else
            {
                ApplicationUser applicationUser = new ApplicationUser()
                {
                    Email = studentVM.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = studentVM.PhoneNumber,
                    UserName = studentVM.FirstName + studentVM.LastName,
                    NormalizedEmail = studentVM.Email.ToUpper(),
                    NormalizedUserName = studentVM.FirstName.ToUpper() + studentVM.LastName.ToUpper()
                };
                IdentityResult user = await _userRepository.CreateUser(applicationUser, studentVM.Password);
                if (!user.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new ErrorResponse
                        {
                            Error = true,
                            Message = "User registration failed!",
                            Field = "Registration"
                        });
                }
                if (!await _userRepository.CheckIfRoleExist("Student"))
                {
                    await _userRepository.CreateUserRole("Student");
                }
                if (await _userRepository.CheckIfRoleExist("Student"))
                {
                    await _userRepository.AssignUserRole(applicationUser, "Student");
                }
                string token = await _userRepository.GetEmailConfirmationToken(applicationUser);
                string url = _configuration["JWT:ValidAudience"] + "/api/User/activate/" + applicationUser.Id + "/" + token;
                await _emailRepository.SendActivationEmail("larry.katuva@gmail.com", "larry.katuva@gmail.com", "Account Activation", url);
                return Ok(new SuccessResponse
                {
                    Error = false,
                    Message = "Student created successfully"
                });
            }
        }


        /// <summary>
        /// Handles incoming request to create a new lecturer
        /// </summary>
        /// <param name="lectureVM"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register-lecturer")]
        public async Task<IActionResult> RegisterLecturer([FromBody] RegisterVM lectureVM)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            ApplicationUser exists = await _userRepository.FindUserByEmail(lectureVM.FirstName);
            if (exists != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new ErrorResponse
                    {
                        Error = true,
                        Message = "Email already registered!",
                        Field = "Email"
                    });
            }
            ApplicationUser applicationUser = new ApplicationUser()
            {
                Email = lectureVM.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                PhoneNumber = lectureVM.PhoneNumber,
                UserName = lectureVM.FirstName + lectureVM.LastName,
                NormalizedEmail = lectureVM.Email.ToUpper(),
                NormalizedUserName = lectureVM.FirstName.ToUpper() + lectureVM.LastName.ToUpper()
            };
            IdentityResult user = await _userRepository.CreateUser(applicationUser, lectureVM.Password);
            if (!user.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ErrorResponse
                    {
                        Error = true,
                        Message = "User registration failed!",
                        Field = "Registration"
                    });
            }
            if (!await _userRepository.CheckIfRoleExist("Lecturer"))
            {
                await _userRepository.CreateUserRole("Lecturer");
            }
            if (await _userRepository.CheckIfRoleExist("Lecturer"))
            {
                await _userRepository.AssignUserRole(applicationUser, "Lecturer");
            }
            return Ok(new SuccessResponse
            {
                Error = false,
                Message = "Lecturer created successfully"
            });
        }



        /// <summary>
        /// Handles incoming request to create a new staff member
        /// </summary>
        /// <param name="staffVM"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register-staff")]
        public async Task<IActionResult> RegisterStaff([FromBody] RegisterVM staffVM)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            ApplicationUser exists = await _userRepository.FindUserByEmail(staffVM.FirstName);
            if (exists != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new ErrorResponse
                    {
                        Error = true,
                        Message = "Email already registered!",
                        Field = "Email"
                    });
            }
            ApplicationUser applicationUser = new ApplicationUser()
            {
                Email = staffVM.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                PhoneNumber = staffVM.PhoneNumber,
                UserName = staffVM.FirstName + staffVM.LastName,
                NormalizedEmail = staffVM.Email.ToUpper(),
                NormalizedUserName = staffVM.FirstName.ToUpper() + staffVM.LastName.ToUpper()
            };
            IdentityResult user = await _userRepository.CreateUser(applicationUser, staffVM.Password);
            if (!user.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ErrorResponse
                    {
                        Error = true,
                        Message = "User registration failed!",
                        Field = "Registration"
                    });
            }
            if (!await _userRepository.CheckIfRoleExist("Staff"))
            {
                await _userRepository.CreateUserRole("Staff");
            }
            if (await _userRepository.CheckIfRoleExist("Staff"))
            {
                await _userRepository.AssignUserRole(applicationUser, "Staff");
            }
            return Ok(new SuccessResponse
            {
                Error = false,
                Message = "Staff member created successfully"
            });
        }



        /// <summary>
        /// Handles incoming request to login user
        /// </summary>
        /// <param name="loginVM"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login-user")]
        public async Task<IActionResult> LoginUser([FromBody] LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            ApplicationUser user = await _userRepository.FindUserByEmail(loginVM.Email);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new ErrorResponse
                    {
                        Error = true,
                        Message = "User email does not exists!",
                        Field = "Email"
                    });
            }
            if(!await _userRepository.LoginUser(user, loginVM.Password))
            {
                return Unauthorized();
            }
            LoginResponse loginResponse = await _userRepository.ClaimUserRoles(user);
            return Ok(loginResponse);
        }

        [HttpGet]
        [Route("get-user-by-Id")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            ApplicationUser user = await _userRepository.FindUserById(userId);
            if(user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new ErrorResponse
                    {
                        Error = true,
                        Message = "User not found",
                        Field = "UserId"
                    });
            }
            return Ok(user);
        }

        [HttpPatch]
        [Route("activate/{userId}/{token}")]
        [AllowAnonymous]
        public async Task<IActionResult> ActivateUserAccount([FromRoute] AccountActivationVM accountActivationVM)
        {
            ApplicationUser user = await _userRepository.FindUserById(accountActivationVM.UserId);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new ErrorResponse
                    {
                        Error = true,
                        Message = "User not found",
                        Field = "UserId"
                    });
            }
            IdentityResult result = await _userRepository.ConfirmEmail(user, accountActivationVM.Token);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new ErrorResponse
                    {
                        Error = true,
                        Message = "Account activativation failed",
                        Field = "Token"
                    });
            }
            return Ok(new SuccessResponse
            {
                Error = false,
                Message = "Account activated successfully"
            });
        }


        [HttpPost]
        [Route("get-password-reset-link")]
        public async Task<IActionResult> PasswordResetLink([FromBody] PasswordResetVM passwordResetVM)
        {
            ApplicationUser user = await _userRepository.FindUserByEmail(passwordResetVM.Email);
            if(user == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new ErrorResponse
                    {
                        Error = true,
                        Message = "Email not found",
                        Field = "Email"
                    });
            }
            string token = await _userRepository.GetPasswordResetToken(user);
            string url = _configuration["JWT:ValidAudience"] + "/api/User/reset-password/" + user.Id + "/" + token;
            await _emailRepository.SendPasswordResetLink("larry.katuva@gmail.com", user.Email, "Password Reset", url);
            return Ok(new SuccessResponse
            {
                Error = false,
                Message = "Reset link has been sent, check email"
            });
        }


        [HttpPatch]
        [Route("update-password")]
        public async Task<IActionResult> UpdatePassword([FromRoute] AccountActivationVM accountActivationVM, [FromForm] UpdatePasswordVM updatePasswordVM)
        {
            ApplicationUser user = await _userRepository.FindUserById(accountActivationVM.UserId);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new ErrorResponse
                    {
                        Error = true,
                        Message = "User not found",
                        Field = "UserId"
                    });
            }
            IdentityResult result = await _userRepository.UpdateUserPassword(user, accountActivationVM.Token, updatePasswordVM.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new ErrorResponse
                    {
                        Error = true,
                        Message = "Password reset failed",
                        Field = "Password"
                    });
            }
            return Ok(new SuccessResponse
            {
                Error = false,
                Message = "Password reset succcessfully"
            });
        }
    }
}
