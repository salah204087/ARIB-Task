using Core.Infrastruture.RepositoryPattern.Repository;
using DataLayer.Models;
using DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Domain.Helper;
using Microsoft.EntityFrameworkCore;
using DataLayer.Helpers;
using Domain.Common;

namespace Domain.Services
{
    public class AccountService(IConfiguration _configuration, ApplicationUserManager _userManager, RoleManager<ApplicationRole> _roleManager, IRepository<Employee> _employeeRepository, ApplicationDbContext _context):IAccountService
    {
        public async Task<UserResult> LoginAsync(LoginDto loginRequestDTO)
        {
            var result = new UserResult();
            var upperUser = loginRequestDTO.Email.ToUpper();
            var user = await _userManager.FindByEmailAsync(upperUser);

            // Check if the user exists and the provided password is valid
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            // If the user is not found or the password is invalid, throw an exception
            if (user == null || !isValid)
                return Helper.Helper.CreateErrorResult<UserResult>(HttpStatusCode.BadRequest, "Email or password is incorrect");
            if (user.IsActive == false)
                return Helper.Helper.CreateErrorResult<UserResult>(HttpStatusCode.BadRequest, "this email is deactivated");
            // Get the roles assigned to the user
            var roles = await _userManager.GetRolesAsync(user);

            // Fetch the user's permissions from the database
           

            // Create a JWT (JSON Web Token) for authentication
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWT:Secret")); // Convert the secret key to bytes

            // Create a token descriptor that includes user claims, token expiration, and signing credentials
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name.ToString()), // Claim for the user's name
                new Claim(ClaimTypes.Email, user.UserName.ToString()), // Claim for the user's email
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Claim for the user's ID
                new Claim("EmployeeId", user.EmployeeId.ToString() ?? ""),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? "") 
            };

            // Add roles to the claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

          

            // Create the token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), // Add all claims here
                Expires = DateTime.UtcNow.AddDays(7), // Token expiration set to 7 days from now
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // HMAC-SHA256 signature
            };

            // Create the JWT token based on the token descriptor
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return a UserDTO containing the user's information and the JWT token
            result.User = new UserDto
            {
                Name = user.Name,
                Email = user.Email,
                Token = tokenHandler.WriteToken(token), // Serialize the token to a string
                Role=roles.ToList(),
            };
            result.SuccessMessage = "Login successful";
            return result;
        }
        public async Task<UserResult> RegisterEmployeeAsync(RegisterationEmployeeDto registerationDto)
        {
            var result = new UserResult();
            var roles = new List<string>();

            await using var transaction = await _context.Database.BeginTransactionAsync(); // Start Transaction

            try
            {
                // Check if the employee already has an account
                if (registerationDto.EmployeeId != null)
                {
                    var employee = await _employeeRepository.GetByIdAsync(registerationDto.EmployeeId);
                    if (employee == null)
                        return Helper.Helper.CreateErrorResult<UserResult>(HttpStatusCode.NotFound, ErrorEnum.NotFoundMessage("Employee"));

                    var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.EmployeeId == registerationDto.EmployeeId);
                    if (existingUser != null)
                        return Helper.Helper.CreateErrorResult<UserResult>(HttpStatusCode.BadRequest, "Employee already has an account.");

                    var employees = await _employeeRepository.FindAllAsync(n => n.ManagerId == employee.Id);
                    if (employees.Any())
                        roles.AddRange(RoleEnum.Manager.ToString(), RoleEnum.Employee.ToString());
                    else
                        roles.Add(RoleEnum.Employee.ToString());
                }

                // Check if a user with the provided email already exists
                var user = await _userManager.FindByEmailAsync(registerationDto.Email);
                if (user != null && user.IsActive == true)
                {
                    return Helper.Helper.CreateErrorResult<UserResult>(HttpStatusCode.Conflict, "User already exists.");
                }
                if (user != null && user.IsActive == false)
                {
                    return Helper.Helper.CreateErrorResult<UserResult>(HttpStatusCode.Conflict, "The user already exists, but the account has been deactivated.");
                }
                // Create a new ApplicationUser object
                ApplicationUser newlyCreatedUser = new ApplicationUser
                {
                    Email = registerationDto.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = registerationDto.Email,
                    Name = registerationDto.Name,
                    NormalizedEmail = registerationDto.Email.ToUpper(),
                    EmployeeId = registerationDto.EmployeeId,
                    IsActive = true
                };

                // Validate password
                var password = registerationDto.Password;
                if (!IsPasswordValid(password) || string.IsNullOrEmpty(password))
                {
                    return Helper.Helper.CreateErrorResult<UserResult>(HttpStatusCode.BadRequest, ErrorEnum.PasswordAuth());
                }

                // Create the user
                var isSuccess = await _userManager.CreateAsync(newlyCreatedUser, registerationDto.Password);

                if (!isSuccess.Succeeded)
                {
                    return Helper.Helper.CreateErrorResult<UserResult>(HttpStatusCode.BadRequest, "Failed to create an account.");
                }

                // Add user to roles
                await _userManager.AddToRolesAsync(newlyCreatedUser, roles);
               

                // Commit transaction if everything succeeds
                await transaction.CommitAsync();

                // Authenticate the newly created user and generate a token
                var token = await AuthenticateAsync(newlyCreatedUser);

                result.User = new UserDto
                {
                    Id = newlyCreatedUser.Id,
                    Token = token,
                    Email = newlyCreatedUser.Email,
                    Name = newlyCreatedUser.Name
                };

                result.SuccessMessage = "User created successfully";
                result.StatusCode = HttpStatusCode.Created;
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Rollback in case of exception
                return Helper.Helper.CreateErrorResult<UserResult>(HttpStatusCode.InternalServerError, "An error occurred while creating the user.");
            }
        }
        public async Task<UserResult> RegisterAdminAsync(RegisterationAdminDto registerationDto)
        {
            var result = new UserResult();
            var roles = new List<string>{ RoleEnum.Admin.ToString() };

            await using var transaction = await _context.Database.BeginTransactionAsync(); 

            try
            {
                // Check if a user with the provided email already exists
                var user = await _userManager.FindByEmailAsync(registerationDto.Email);
                if (user != null && user.IsActive == true)
                {
                    return Helper.Helper.CreateErrorResult<UserResult>(HttpStatusCode.Conflict, "User already exists.");
                }
                if (user != null && user.IsActive == false)
                {
                    return Helper.Helper.CreateErrorResult<UserResult>(HttpStatusCode.Conflict, "The user already exists, but the account has been deactivated.");
                }
                // Create a new ApplicationUser object
                ApplicationUser newlyCreatedUser = new ApplicationUser
                {
                    Email = registerationDto.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = registerationDto.Email,
                    Name = registerationDto.Name,
                    NormalizedEmail = registerationDto.Email.ToUpper(),
                    IsActive = true
                };

                // Validate password
                var password = registerationDto.Password;
                if (!IsPasswordValid(password) || string.IsNullOrEmpty(password))
                {
                    return Helper.Helper.CreateErrorResult<UserResult>(HttpStatusCode.BadRequest, ErrorEnum.PasswordAuth());
                }

                // Create the user
                var isSuccess = await _userManager.CreateAsync(newlyCreatedUser, registerationDto.Password);

                if (!isSuccess.Succeeded)
                {
                    return Helper.Helper.CreateErrorResult<UserResult>(HttpStatusCode.BadRequest, "Failed to create an account.");
                }

                // Add user to roles
                await _userManager.AddToRolesAsync(newlyCreatedUser, roles);


                // Commit transaction if everything succeeds
                await transaction.CommitAsync();

                // Authenticate the newly created user and generate a token
                var token = await AuthenticateAsync(newlyCreatedUser);

                result.User = new UserDto
                {
                    Id = newlyCreatedUser.Id,
                    Token = token,
                    Email = newlyCreatedUser.Email,
                    Name = newlyCreatedUser.Name
                };

                result.SuccessMessage = "User created successfully";
                result.StatusCode = HttpStatusCode.Created;
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Rollback in case of exception
                return Helper.Helper.CreateErrorResult<UserResult>(HttpStatusCode.InternalServerError, "An error occurred while creating the user.");
            }
        }
        public bool IsPasswordValid(string password)
        {

            if (password.Length > 8 && password.Length < 20)
            {
                bool hasUppercase = password.Any(char.IsUpper);
                bool hasLowercase = password.Any(char.IsLower);
                bool hasDigit = password.Any(char.IsDigit);
                bool hasSpecialCharacter = password.Any(ch => !char.IsLetterOrDigit(ch));

                return hasUppercase && hasLowercase && hasDigit && hasSpecialCharacter;
            }
            return false;
        }
        public async Task<string> AuthenticateAsync(ApplicationUser user)
        {
            // Get the roles associated with the user using the UserManager
            var roles = await _userManager.GetRolesAsync(user);

            // Create an array of claims to represent user information
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // User's unique identifier
                new Claim(ClaimTypes.Email, user.Email), // User's email
                new Claim(ClaimTypes.Name, user.Name), // User's name
                new Claim(ClaimTypes.Role, roles.FirstOrDefault()), // User's role
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID claim
            };

            // Create a security key for signing the JWT, using the secret key from configuration
            var authSignedKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            // Create a JWT (JSON Web Token) with issuer, audience, claims, expiration, and signing credentials
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"], // The issuer of the token
                audience: _configuration["JWT:Audience"], // The intended audience of the token
                claims: claims, // User claims
                expires: DateTime.Now.AddDays(30), // Token expiration (e.g., 30 days from now)
                signingCredentials: new SigningCredentials(authSignedKey, SecurityAlgorithms.HmacSha256) // Signing credentials
            );

            // Return the JWT token as a serialized string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public List<string> GetRoles()
        {
            var roles = typeof(RoleEnum).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                                      .Select(f => f.GetValue(null)?.ToString())
                                      .ToList();
            return roles;
        }
        public async Task<OperationResult> ChangePasswordAsync(string email, string currentPassword, string newPassword)
        {
            var result = new OperationResult();
            // Find the user based on the provided email
            var user = await _userManager.FindByEmailAsync(email);

            // Check if the user exists and the provided current password is valid
            bool isValidPassword = await _userManager.CheckPasswordAsync(user, currentPassword);

            // If the user is not found or the current password is invalid, throw an exception
            if (user == null || !isValidPassword)
            {
                return Helper.Helper.CreateErrorResult<OperationResult>(HttpStatusCode.NotFound, "user is not found or the current password is invalid");
            }
            if (user.IsActive == false)
            {
                return Helper.Helper.CreateErrorResult<UserResult>(HttpStatusCode.BadRequest, "The account has been deactivated, you don't able to change password.");
            }
            // Check if the new password is valid
            if (!IsPasswordValid(newPassword))
            {
                return Helper.Helper.CreateErrorResult<OperationResult>(HttpStatusCode.BadRequest, ErrorEnum.PasswordAuth());
            }
            var isAdmin = await _userManager.IsInRoleAsync(user, "admin");

            // If the user has the "admin" role, throw an exception
            if (isAdmin)
            {
                return Helper.Helper.CreateErrorResult<OperationResult>(HttpStatusCode.Unauthorized, "Changing password for admin user is not allowed");
            }
            // Change the user's password using UserManager
            var status = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            // If the password change is successful, return true
            if (status.Succeeded)
            {
                result.SuccessMessage = "Password changed successfully";
                return result;
            }
            else
            {
                // If password change failed, throw an exception or handle accordingly
                return Helper.Helper.CreateErrorResult<OperationResult>(HttpStatusCode.BadRequest, "Failed to change the password");
            }
        }
        public async Task<OperationResult> ChangeStatusOfAccount(string email, bool active)
        {
            var result = new OperationResult();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Helper.Helper.CreateErrorResult<OperationResult>(HttpStatusCode.NotFound, "Didn't find the user");
            if (user.IsActive == true && active == true)
                return Helper.Helper.CreateErrorResult<OperationResult>(HttpStatusCode.BadRequest, "User account is already active");
            if (user.IsActive == false && active == false)
                return Helper.Helper.CreateErrorResult<OperationResult>(HttpStatusCode.BadRequest, "User account is already deactivated");
            user.IsActive = active;
            await _userManager.UpdateAsync(user);
            result.SuccessMessage = "User account status changed successfully.";
            return result;
        }
        public async Task<BasicUserResult> GetAllUsersAsync()
        {
            var result = new BasicUserResult();
            var users = await _userManager.Users.ToListAsync();

            result.UsersDto = users.Select(u => new GetBasicUserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                IsActive = u.IsActive
            }).ToList();

            result.SuccessMessage = "Get User successfully";
            return result;
        }
        public async Task<DetailUserResult> GetUserByIdAsync(int userId)
        {
            var result = new DetailUserResult();
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return Helper.Helper.CreateErrorResult<DetailUserResult>(HttpStatusCode.NotFound, ErrorEnum.NotFoundMessage("DetailUser"));

            var roles = await _userManager.GetRolesAsync(user);
           
            result.GetDetailedUser = new GetDetailedUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                EmployeeId = user.EmployeeId,
                IsActive = user.IsActive,
                Roles = roles.ToList()
            };
            result.SuccessMessage = MessageEnum.Getted(nameof(GetDetailedUserDto));
            return result;
        }
    }
}
