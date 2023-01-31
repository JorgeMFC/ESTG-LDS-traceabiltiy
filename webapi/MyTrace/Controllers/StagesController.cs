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
    public class StagesController : ControllerBase
    {
        private readonly MyTraceContext _context;
        private readonly UserData _userGetter;
        JwtAuthenticationManager _jwtAuthenticationManager;

        public StagesController(MyTraceContext context, JwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _userGetter = new UserData(context);
            _jwtAuthenticationManager = jwtAuthenticationManager;   
        }


        /**<summary>
         * Get all stages of a specific organization
         * </summary>
         * <param name="idOrg">The organization id</param>
         */
        [HttpGet]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Pager<Stage>>> GetStages([FromQuery][BindRequired] int page, [FromQuery][BindRequired] int perPage, [FromQuery] string? search)  
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

            var stages = await _context.Stages
                .Where(s => s.OrganizationId == idOrg)
                .Where(s => search == null || s.StageName.Contains(search))
                .ToListAsync();

            Pager<Stage> stagesPaginator = new Pager<Stage>
            (
                    stages,
                    perPage,
                    page
                );

            return Ok(stagesPaginator);
        }

        /**<summary>
         * Get a stage by id and organizationId
         * </summary>
         * <param name="id">The stage id</param>
         * <param name="idOrg">The organization id</param>
         */
        [HttpGet("getStagesById")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Stage>>> GetStagesById([FromQuery][BindRequired] string id)
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

            var stage = await _context.Stages.FindAsync(id, idOrg);

            if (stage is null)
            {
                throw new StageArgumentException("Stage Not Found!");
            }

            return Ok(stage);
        }

        /** <summary>
         * Add new stage
         * </summary>
         * <param name="stage">The stage to be added</param>
         */
        [HttpPost]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Stage>>> AddStage(Stage stage)
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
                StageRequestValidator.Validate(stage);

                stage.OrganizationId = idOrg;

                if (await _context.Stages.FindAsync(stage.Id, stage.OrganizationId) != null)
                {
                    throw new StageArgumentException("Stage already exists!");
                }

                _context.Stages.Add(stage);
                await _context.SaveChangesAsync();

                return Ok(await _context.Stages.ToListAsync());
            }
            catch (StageArgumentException exception)
            {
                throw new StageArgumentException(exception.Message);
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /** <summary>
         * Update an existant Stage
         * </summary>
         * <param name="stage">The stage to be updated</param>
         */
        [HttpPut]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Stage>>> UpdateStage(Stage stage)
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
                StageRequestValidator.Validate(stage);

                stage.OrganizationId = idOrg;

                var result = await _context.Stages.FindAsync(stage.Id, stage.OrganizationId);

                if (result == null)
                {
                    throw new StageArgumentException("Stage not found!");
                }

                result.Id = stage.Id;
                result.OrganizationId = stage.OrganizationId;
                result.StageName = stage.StageName;
                result.StageDescription = stage.StageDescription;   
                result.StagesTypeId =  stage.StagesTypeId;

                await _context.SaveChangesAsync();
                return Ok(await _context.Users.ToListAsync());
            }
            catch (StageArgumentException exception)
            {
                throw new StageArgumentException(exception.Message);
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary>
         * Delete a stage only if no model is associated with it
         * </summary>
         * <param name="id">The stage id to be deleted</param>
         * <param name="idOrg">The organization id of stage to be deleted</param>
         */
        [HttpDelete]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Stage>>> DeleteStage([FromQuery][BindRequired] string id)
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

            var stage = await _context.Stages.FindAsync(id, idOrg);      

            if (stage == null)
            {
                throw new StageArgumentException("Stage not found!");
            }

            var stagesModelUsing = _context.StagesModelStages
                    .Where(s => s.StagesId == id);

            if (stagesModelUsing.Any())
            {
                throw new StageArgumentException("This stage can't be deleted because some model is using it! Please check and try again.");
            }

            _context.Stages.Remove(stage);
            await _context.SaveChangesAsync();

            return Ok(await _context.Stages.ToListAsync());
        }

        /** <summary>
        * Get all stages of a specific type
        * </summary>
        * <param name="id">The id that identify the stage type</param>
        */
        [HttpGet("getStagesByType")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Stage>>> getStagesByType([FromQuery][BindRequired] string id)
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

            var result = await _context.StagesTypes.FindAsync(id);

            if (result == null)
            {
                throw new StageArgumentException("User type not found!");
            }

            var stages = _context.Stages
                    .Where(s => s.StagesTypeId == id && s.OrganizationId == idOrg);

            return Ok(stages);
        }

        /**<summary>
         * Get stages type by id
         * </summary>
         * <param name="id">The stage type id</param>
         */
        [HttpGet("getStagesTypeById")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<StagesType>> getStagesTypeById([FromQuery][BindRequired] string id)
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

            var result = await _context.StagesTypes.FindAsync(id, idOrg);

            if (result == null)
            {
                throw new StageArgumentException("Stage type doesn't exist!");
            }

            return Ok(result);
        }

        /**<summary>
         * Get all stages types
         * </summary>
         */
        [HttpGet("getStagesTypes")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<StagesType>>> getStagesTypes()
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

            return Ok(await _context.StagesTypes.ToListAsync());
        }
    }
}
