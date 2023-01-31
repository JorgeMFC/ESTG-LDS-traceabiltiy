using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using MyTrace.Models;
using MyTrace.Utils;
using Nethereum.JsonRpc.Client;
using MyTrace.Domain;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;

namespace MyTrace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyTraceContext _context;
        private readonly UserData _userGetter;
        JwtAuthenticationManager _jwtAuthenticationManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(MyTraceContext context, JwtAuthenticationManager jwtAuthenticationManager, IWebHostEnvironment webHostEnvironment)
        {
              _context = context;
              _userGetter = new UserData(context);
              _jwtAuthenticationManager = jwtAuthenticationManager;
              _webHostEnvironment =webHostEnvironment;
        }

        [HttpGet]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Pager<UserResponse>>> GetUsers([FromQuery][BindRequired] int page, [FromQuery][BindRequired] int perPage, [FromQuery] string? search, [FromQuery] int? idOrg, [FromQuery] byte? idRole, [FromQuery] string? startDate, [FromQuery] string? endDate)
        {

            string? wallet = _jwtAuthenticationManager.GetUserWallet(
              HttpContext.Request.Headers["Authorization"]
           );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }
  
            var requestUser = await _userGetter.GetUserAsync(wallet);

            if (requestUser == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var currentUserRole = await _context.UsersTypes.FindAsync(requestUser.UserTypeId);

            if (currentUserRole != null && currentUserRole.UserType != "Owner" && currentUserRole.UserType != "Manager" && currentUserRole.UserType != "Admin")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var organizationId = requestUser.OrganizationId;
            var requestUserOrganization = await _context.Organizations.FindAsync(organizationId);

            if (requestUserOrganization == null)
            {
                throw new ModelArgumentException("Organization Not Found!");
            }

            if (currentUserRole != null && (currentUserRole.UserType == "Owner" || currentUserRole.UserType == "Manager"))
            {
                idOrg = requestUser.OrganizationId;
            }

            List<User> users = new List<User>();

            users = await _context.Users
                                       .Where(u => search == null || u.Name == null || u.Email == null || (u.Name.Contains(search) || u.Email.Contains(search)))
                                       .Where(u => startDate == null || u.CreatedAt.Date >= DateTime.Parse(startDate).Date)
                                       .Where(u => endDate == null || u.CreatedAt.Date <= DateTime.Parse(endDate).Date)
                                       .Where(u => idRole == null || u.UserTypeId == idRole)
                                       .Where(u => idOrg == null || u.OrganizationId == idOrg)
                                       .ToListAsync();

            if (users == null)
            {
                throw new UserArgumentException("Any user found in the system!");
            }

            List<UserResponse> userResponses = new List<UserResponse>();

            foreach (var user in users)
            {
                UserResponse userResponse = new UserResponse();

                userResponse.User = user;
                var userType = await _context.UsersTypes.FindAsync(user.UserTypeId);
                var organization = await _context.Organizations.FindAsync(user.OrganizationId);

                if (userType != null)
                {
                    userResponse.Role = userType.UserType;                 
                }

                if (organization != null)
                {
                    userResponse.NameOrg = organization.Name;
                }

                userResponses.Add(userResponse);
            }

            Pager<UserResponse> usersPaginator = new Pager<UserResponse>
                (
                    userResponses,
                    perPage,
                    page
                );


            return Ok(usersPaginator);
        }

        /**<summary>
         * Return users by status.
         * If status is 0, return all desactivated users.
         * If status is 1, return all activated users.
         * </summary> 
         * <param name="status">The user account status</param>
         * */
        [HttpGet("getUserInfo")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<User>> GetUserInfo()
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var user = await _userGetter.GetUserAsync(wallet);

            if (user == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            return Ok(user);
        }

        /**<summary>
         * Return users by status.
         * If status is 0, return all desactivated users.
         * If status is 1, return all activated users.
         * </summary> 
         * <param name="status">The user account status</param>
         * */
        [HttpGet("getUsersByStatus")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Pager<User>>> GetUsersByStatus([FromQuery][BindRequired] int status, [FromQuery][BindRequired] int page, [FromQuery][BindRequired] int perPage)
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var user = await _userGetter.GetUserAsync(wallet);

            if (user == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var userType = await _context.UsersTypes.FindAsync(user.UserTypeId);

            if (userType != null && userType.UserType != "Owner" && userType.UserType != "Manager" && userType.UserType != "Admin")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            int? idOrg = null;

            if (user.OrganizationId != null && userType != null && (userType.UserType == "Owner" || userType.UserType == "Manager"))
            {
                idOrg = (int)user.OrganizationId;

                var organization = await _context.Organizations.FindAsync(idOrg);

                if (organization == null)
                {
                    throw new ModelArgumentException("Organization Not Found!");
                }
            }
            
            List<User> users = new List<User>();

            if (status == 0)
            {
                users = await _context.Users
                    .Where(u => u.DeletedAt != null)
                    .Where(u => idOrg == null || u.OrganizationId == idOrg)
                    .ToListAsync();
            } 
            else
            {
                users = await _context.Users
                    .Where(u => u.DeletedAt == null)
                    .Where(u => idOrg == null || u.OrganizationId == idOrg)
                    .ToListAsync();
            }

            Pager<User> usersPaginator = new Pager<User>
            (
                    users,
                    perPage,
                    page
                );

            return Ok(usersPaginator);
        }

        /**<summary>
         * Return an user by wallet
         * </summary> 
         * <param name="wallet">the user wallet</param>
         */
        [HttpGet("getUserByWallet")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<User>> GetUserByWallet([FromQuery][BindRequired] string wallet)
        {
            string? walletFromBody = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (walletFromBody == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var user = await _context.Users.FindAsync(wallet);
            if (user == null)
            {
                throw new UserArgumentException("User Not Found!");
            }

            return Ok(user);
        }

        /*
        [HttpGet("getUsersByOrganization")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Pager<UserResponse>>> GetUsersOrg([FromQuery][BindRequired] int page, [FromQuery][BindRequired] int perPage, [FromQuery] string? search, [FromQuery] byte? idRole, [FromQuery] string? startDate, [FromQuery] string? endDate)
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var user1 = await _userGetter.GetUserAsync(wallet);

            if (user1 == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var idOrg = user1.OrganizationId;
            var organization1 = await _context.Organizations.FindAsync(idOrg);

            if (organization1 == null)
            {
                throw new ModelArgumentException("Organization Not Found!");
            }

            List<User> users = new List<User>();

            users = await _context.Users
                                       .Where(u => u.OrganizationId == idOrg)
                                       .Where(u => search == null || u.Name == null || u.Email == null || (u.Name.Contains(search) || u.Email.Contains(search)))
                                       .Where(u => startDate == null || u.CreatedAt.Date >= DateTime.Parse(startDate).Date)
                                       .Where(u => endDate == null || u.CreatedAt.Date <= DateTime.Parse(endDate).Date)
                                       .Where(u => idRole == null || u.UserTypeId == idRole)
                                       .ToListAsync();


            if (users == null)
            {
                throw new UserArgumentException("Any User Found in this organization!");
            }

            List<UserResponse> userResponses = new List<UserResponse>();

            foreach (var user in users)
            {
                UserResponse userResponse = new UserResponse();

                userResponse.User = user;
                var userType = await _context.UsersTypes.FindAsync(user.UserTypeId);
                var organization = await _context.Organizations.FindAsync(user.OrganizationId);

                if (userType != null)
                {
                    userResponse.Role = userType.UserType;
                }

                if (organization != null)
                {
                    userResponse.NameOrg = organization.Name;
                }

                userResponses.Add(userResponse);
            }

            Pager<UserResponse> usersPaginator = new Pager<UserResponse>
                (
                    userResponses,
                    perPage,
                    page
                );

            return Ok(usersPaginator);
        }
        */

        /** <summary>
         * Return all desactivated users
         * </summary>
         * */
        [HttpGet("getDesactivatedUsers")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<User>> GetDesactivatedUsers([FromQuery][BindRequired] int page, [FromQuery][BindRequired] int perPage)
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var user = await _userGetter.GetUserAsync(wallet);

            if (user == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var userType = await _context.UsersTypes.FindAsync(user.UserTypeId);

            if (userType != null && userType.UserType != "Owner" && userType.UserType != "Manager" && userType.UserType != "Admin")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            int? idOrg = null;

            if (user.OrganizationId != null && userType != null && (userType.UserType == "Owner" || userType.UserType == "Manager"))
            {
                idOrg = (int)user.OrganizationId;

                var organization = await _context.Organizations.FindAsync(idOrg);

                if (organization == null)
                {
                    throw new ModelArgumentException("Organization Not Found!");
                }
            }

            var users = await _context.Users
                    .Where(u => u.DeletedAt != null)
                    .Where(u => idOrg == null || u.OrganizationId == idOrg)
                    .ToListAsync();

            Pager<User> usersPaginator = new Pager<User>
                (
                    users,
                    perPage,
                    page
                );


            return Ok(usersPaginator);
        }

        /**<summary>
         * Change role of an existant user
         * </summary>
         * <param name="userEmail">the user email</param>
         * <param name="role">the new role to be setted</param>
         */
        [HttpPatch("changeUserRole")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<User>>> ChangeUserRole([FromQuery][BindRequired] string userEmail, [FromQuery][BindRequired] byte role)
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var requestUser = await _userGetter.GetUserAsync(wallet);

            if (requestUser == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var userType = await _context.UsersTypes.FindAsync(requestUser.UserTypeId);

            if (userType != null && userType.UserType != "Owner" && userType.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var idOrg = requestUser.OrganizationId;
            var organization = await _context.Organizations.FindAsync(idOrg);

            if (organization == null)
            {
                throw new ModelArgumentException("Organization Not Found!");
            }

            try
            {
                try
                {
                    var user = await _context.Users
                                        .Where(u => u.Email == userEmail)
                                        .SingleAsync();
               
                    if (user.OrganizationId != null)
                    {
                        throw new UserArgumentException("The specified user is already associated to an organization!");
                    }

                    var currentUserRole = await _context.UsersTypes.FindAsync(user.UserTypeId);

                    if(currentUserRole != null && currentUserRole.UserType == "Owner")
                    {
                        throw new UserArgumentException("An organization can´t be without an owner!");
                    }

                    var newUserRole = await _context.UsersTypes.FindAsync(role);

                    if (newUserRole == null)
                    {
                        throw new UserArgumentException("Invalid role! Specified role doesn't exist");
                    }

                    user.UserTypeId = role;
                    user.OrganizationId = idOrg;

                    await _context.SaveChangesAsync();

                    return Ok(await _context.Users.ToListAsync());

                }
                catch (InvalidOperationException)
                {
                    throw new UserArgumentException("User not found!");
                }
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary>
        * Remove an user from an organizationl
        * </summary>
        * <param name="wallet">The wallet of user to be removed</param>
        */
        [HttpPatch("removeUserFromOrganization")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<User>>> RemoveUserFromOrganization([FromQuery][BindRequired] string wallet)
        {
            string? walletFromBody = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (walletFromBody == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var requestUser = await _userGetter.GetUserAsync(wallet);

            if (requestUser == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var currentRole = await _context.UsersTypes.FindAsync(requestUser.UserTypeId);

            if (currentRole != null && currentRole.UserType != "Owner" && currentRole.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var idOrg = requestUser.OrganizationId;
            var organization = await _context.Organizations.FindAsync(idOrg);

            if (organization == null)
            {
                throw new ModelArgumentException("Organization Not Found!");
            }

            try
            {
                var result = await _context.Users.FindAsync(wallet);

                if (result == null)
                {
                    throw new UserArgumentException("User not found!");
                }

                if (result.DeletedAt != null)
                {
                    throw new UserArgumentException("This user is already desactivated");
                }

                var currentUserRole = await _context.UsersTypes.FindAsync(result.UserTypeId);

                if (currentUserRole != null && currentUserRole.UserType == "Owner")
                {
                    throw new UserArgumentException("An owner can´t be removed!");
                }

                if (result.OrganizationId != null)
                {
                    result.OrganizationId = null;

                    UsersType userType;

                    try
                    {
                        userType = await _context.UsersTypes
                       .Where(ut => ut.UserType == "User").SingleAsync();
                    }
                    catch (InvalidOperationException)
                    {
                        throw new InvalidOperationException("Role user not found!");
                    }

                    result.UserTypeId = userType.Id;
                }

                await _context.SaveChangesAsync();

                return Ok(await _context.Users.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /** <summary>
         * Update an existant user
         * </summary>
         *<param name="user">the user to be updated</param> 
         */
        [HttpPut]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<User>>> UpdateUser([Required][FromForm] UserRequest userRequest)
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var requestUser = await _userGetter.GetUserAsync(wallet);

            if (requestUser == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var currentRole = await _context.UsersTypes.FindAsync(requestUser.UserTypeId);

            if (currentRole != null && currentRole.UserType != "Owner" && currentRole.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var idOrg = requestUser.OrganizationId;
            var organization = await _context.Organizations.FindAsync(idOrg);

            if (organization == null)
            {
                throw new ModelArgumentException("Organization Not Found!");
            }

            try
            {
                var user = userRequest.User;

                UserRequestValidator.Validate(user);

                var result = await _context.Users.FindAsync(user.WalletAddress);

                if (result == null)
                {
                    return NotFound("User not found!");
                }

                result.WalletAddress = user.WalletAddress;
                result.Nonce = user.Nonce;
                result.Name = user.Name;
                result.Email = user.Email;
                result.BirthDate = user.BirthDate;    
                result.UserTypeId = user.UserTypeId;    
                result.DeletedAt = user.DeletedAt;

                if (userRequest.Avatar != null)
                {
                    Images images = new Images(_webHostEnvironment);
                    string? urlImage = await images.saveImage(userRequest.Avatar);

                    if (urlImage == null)
                    {
                        throw new UserArgumentException("Invalid image");
                    }

                    result.Avatar = urlImage;
                }

                await _context.SaveChangesAsync();

                return Ok(await _context.Users.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /** <summary>
         * Desactivate user account
         * </summary>
         * <param name="wallet">wallet of user to be desactivated</param>
         */
        [HttpPatch("desactivateUser")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<User>>> DesactivateUser([FromQuery][BindRequired] string wallet)
        {
            string? walletFromBody = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (walletFromBody == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var requestUser = await _userGetter.GetUserAsync(wallet);

            if (requestUser == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var currentRole = await _context.UsersTypes.FindAsync(requestUser.UserTypeId);

            if (currentRole != null && currentRole.UserType != "Owner" && currentRole.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var idOrg = requestUser.OrganizationId;
            var organization = await _context.Organizations.FindAsync(idOrg);

            if (organization == null)
            {
                throw new ModelArgumentException("Organization Not Found!");
            }

            try
            {
                var result = await _context.Users.FindAsync(wallet);
               
                if (result == null)
                {
                    throw new UserArgumentException("User not found!");
                }

                result.DeletedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(await _context.Users.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary>
         * Activate an user
         * </summary>
         * <param name="wallet">The wallet of user to be activated</param>
         */
        [HttpPatch("activateUser")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<User>>> ActivateUser([FromQuery][BindRequired] string wallet)
        {
            string? walletFromBody = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (walletFromBody == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var requestUser = await _userGetter.GetUserAsync(wallet);

            if (requestUser == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var currentRole = await _context.UsersTypes.FindAsync(requestUser.UserTypeId);

            if (currentRole != null && currentRole.UserType != "Owner" && currentRole.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var idOrg = requestUser.OrganizationId;
            var organization = await _context.Organizations.FindAsync(idOrg);

            if (organization == null)
            {
                throw new ModelArgumentException("Organization Not Found!");
            }

            try
            {
                var result = await _context.Users.FindAsync(wallet);

                if (result == null)
                {
                    throw new UserArgumentException("User not found!");
                }

                if (result.DeletedAt == null)
                {
                    throw new UserArgumentException("This user is already active");
                }

                result.DeletedAt = null;

                await _context.SaveChangesAsync();

                return Ok(await _context.Users.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /** <summary>
         * Get all users of a specific type
         * </summary>
         * <param name="id">The id that identify the user type</param>
         */
        [HttpGet("getUsersByType")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<User>>> GetUsersByType([FromQuery][BindRequired] byte id, [FromQuery][BindRequired] int page, [FromQuery][BindRequired] int perPage)
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var requestUser = await _userGetter.GetUserAsync(wallet);

            if (requestUser == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var currentRole = await _context.UsersTypes.FindAsync(requestUser.UserTypeId);

            if (currentRole != null && currentRole.UserType != "Owner" && currentRole.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var idOrg = requestUser.OrganizationId;
            var organization = await _context.Organizations.FindAsync(idOrg);

            if (organization == null)
            {
                throw new ModelArgumentException("Organization Not Found!");
            }

            var result = await _context.UsersTypes.FindAsync(id);

            if (result == null)
            {
                throw new UserArgumentException("User type not found!");
            }

            var users = _context.Users
                    .Where(u => u.UserTypeId == id && u.OrganizationId == idOrg);

            Pager<User> usersPaginator = new Pager<User>
                (
                    users,
                    perPage,
                    page
                );


            return Ok(usersPaginator);
        }

        /**<summary>
         * Get User Type by id
         * </summary>
         * <param name="id">The id that identify the user type</param>
         */
        [HttpGet("getUsersTypeById")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<UsersType>> GetUsersTypeById([FromQuery][BindRequired] byte id)
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var requestUser = await _userGetter.GetUserAsync(wallet);

            if (requestUser == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var currentRole = await _context.UsersTypes.FindAsync(requestUser.UserTypeId);

            if (currentRole != null && currentRole.UserType != "Owner" && currentRole.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var idOrg = requestUser.OrganizationId;
            var organization = await _context.Organizations.FindAsync(idOrg);

            if (organization == null)
            {
                throw new ModelArgumentException("Organization Not Found!");
            }

            var result = await _context.UsersTypes.FindAsync(id);

            if (result == null)
            {
                throw new UserArgumentException("User type doesn't exist!");
            }

            return Ok(result);
        }

        /**<summary>
         * Get all user types
         * </summary>
         */
        [HttpGet("getUsersTypes")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<UsersType>>> GetUsersTypes()
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            var requestUser = await _userGetter.GetUserAsync(wallet);

            if (requestUser == null)
            {
                throw new AuthenticationArgumentException("Invalid user");
            }

            var currentRole = await _context.UsersTypes.FindAsync(requestUser.UserTypeId);

            if (currentRole != null && currentRole.UserType != "Owner" && currentRole.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var idOrg = requestUser.OrganizationId;
            var organization = await _context.Organizations.FindAsync(idOrg);

            if (organization == null)
            {
                throw new ModelArgumentException("Organization Not Found!");
            }

            return Ok(await _context.UsersTypes.ToListAsync());
        }
    }
}
