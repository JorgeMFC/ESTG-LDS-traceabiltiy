using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
﻿using Microsoft.AspNetCore.Authorization;
using MyTrace.Models;
using MyTrace.Utils;
using MyTrace.Domain;

namespace MyTrace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StagesModelController : Controller
    {
        /**<summary> 
        * Context of database
        * </summary>
        */
        private readonly MyTraceContext _context;
        private readonly UserData _userGetter;
        JwtAuthenticationManager _jwtAuthenticationManager;

        /**<summary> 
         * ComponentController Builder 
         * </summary>
         */
        public StagesModelController(MyTraceContext context, JwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _userGetter = new UserData(context);
            _jwtAuthenticationManager = jwtAuthenticationManager;   
        }

        /**<summary> 
        * Function that returns an valid stagesModel with your id and idOrganization
        * </summary>
        * <param name="id"> stagesModelId </param>
        * <param name="idOrg>"> OrganizationId </param>
        */
        [HttpGet("getStagesModel")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<StagesModel>>> GetStagesModel([FromQuery][BindRequired] string id)
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

            if (userType != null && userType.UserType != "Owner" && userType.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var idOrg = user.OrganizationId;
            var organization = await _context.Organizations.FindAsync(idOrg);

            if (organization == null)
            {
                throw new ModelArgumentException("Organization Not Found!");
            }

            return Ok(await _context.StagesModels.FindAsync(id, idOrg));
        }

        /**<summary> 
        * Function that returns all valid stagesModel from a organization
        * </summary>
        * <param name="idOrg"> OrganizationId </param>
        */
        [HttpGet]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Pager<StagesModel>>> GetStagesModels([FromQuery][BindRequired] int page, [FromQuery][BindRequired] int perPage, [FromQuery] string? search)
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

            if (userType != null && userType.UserType != "Owner" && userType.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var idOrg = user.OrganizationId;
            var organization = await _context.Organizations.FindAsync(idOrg);

            if (organization == null)
            {
                throw new ModelArgumentException("Organization Not Found!");
            }

            var stagesModels = await _context.StagesModels
                                        .Where(sm => search == null || (sm.StagesModelName.Contains(search)))
                                        .Where(sm => sm.OrganizationId == idOrg)
                                        .ToListAsync();

            Pager<StagesModel> stagesModelPaginator = new Pager<StagesModel>
            (
                    stagesModels,
                    perPage,
                    page
                );

            return Ok(stagesModelPaginator);
        }

        /**<summary> 
         * Function that insert a new valid stagesModel in the database
         * </summary>
         * <param name="stagesModel"> The stagesModel to add </param>
         */
        [HttpPost]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<StagesModel>>> AddStagesModel(StagesModel stagesModel)
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

            if (userType != null && userType.UserType != "Owner" && userType.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var idOrg = (int)user.OrganizationId;
            var organization = await _context.Organizations.FindAsync(idOrg);

            if (organization == null)
            {
                throw new ModelArgumentException("Organization Not Found!");
            }

            try
            {
                StagesModelRequestValidator.Validate(stagesModel);

                stagesModel.OrganizationId = idOrg;

                if (await _context.StagesModels.FindAsync(stagesModel.Id, stagesModel.OrganizationId) != null)
                {
                    throw new StagesModelArgumentException("StagesModel allready exist!!");
                }

                _context.StagesModels.Add(stagesModel);
                await _context.SaveChangesAsync();

                return Ok(await _context.StagesModels.FindAsync(stagesModel.Id, stagesModel.OrganizationId));
            }
            catch (DbUpdateException)
            {
                throw new StagesModelArgumentException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary> 
         * Function that updates an existing stagesModel with its new data and OrganizationId
         * </summary>
         * <param name="stagesModel"> The new data of stagesModel </param>
         */
        [HttpPut]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<StagesModel>>> UpdateStagesModel(StagesModel stagesModel)
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

            if (userType != null && userType.UserType != "Owner" && userType.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var idOrg = (int)user.OrganizationId;
            var organization = await _context.Organizations.FindAsync(idOrg);

            if (organization == null)
            {
                throw new ModelArgumentException("Organization Not Found!");
            }

            try
            {
                StagesModelRequestValidator.Validate(stagesModel);

                stagesModel.OrganizationId = idOrg;

                var result = await _context.StagesModels.FindAsync(stagesModel.Id, stagesModel.OrganizationId);
                if (result == null)
                {
                    throw new StagesModelArgumentException("ComponentType not found.");
                }

                result.StagesModelName = stagesModel.StagesModelName;

                await _context.SaveChangesAsync();

                return Ok(await _context.StagesModels.FindAsync(stagesModel.Id, stagesModel.OrganizationId));
            }
            catch (DbUpdateException)
            {
                throw new StagesModelArgumentException("Some specified Id's are invalid! Please check again!s");
            }
        }

        /**<summary> 
         * Function that remove an existing stagesModel with your Id adn OrganizationId
         * <summary>
         * <param name="id"> stagesModelId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpDelete]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<StagesModel>>> RemoveStagesModel([FromQuery][BindRequired] string id)
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

            if (userType != null && userType.UserType != "Owner" && userType.UserType != "Manager")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var idOrg = user.OrganizationId;
            var organization = await _context.Organizations.FindAsync(idOrg);

            if (organization == null)
            {
                throw new ModelArgumentException("Organization Not Found!");
            }

            try
            {
                var result = await _context.StagesModels.FindAsync(id, idOrg);
                if (result == null)
                {
                    throw new StagesModelArgumentException("StagesModel not found.");
                }

                _context.StagesModels.Remove(result);
                await _context.SaveChangesAsync();

                return Ok(await _context.StagesModels.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new StagesModelArgumentException("You cant delete this stagesModel, maybe exist Model with this stagesModel");
            }
        }
    }
}
