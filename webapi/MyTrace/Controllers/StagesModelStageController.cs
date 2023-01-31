using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using MyTrace.Models;
using MyTrace.Utils;
using Nethereum.RPC.Eth.Filters;
using MyTrace.Domain;

namespace MyTrace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StagesModelStageController : Controller
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
        public StagesModelStageController(MyTraceContext context, JwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _userGetter = new UserData(context);
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        /**<summary> 
        * Function that returns AN valid stagesModelStage
        * </summary>
        * <param name="id"> stagesModelStageId </param>
        * <param name="idOrg>"> OrganizationId </param>
        */
        [HttpGet("getStagesModelStage")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Pager<StagesModelStage>>> GetStagesModelStage([FromQuery][BindRequired] string id, [FromQuery][BindRequired] int page, [FromQuery][BindRequired] int perPage)
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

            var stagesModelStage = await _context.StagesModelStages
                .Where(p => p.OrganizationId == idOrg && p.StagesModelId == id)
                .OrderBy(p => p.Position)
                .ToListAsync();

            Pager<StagesModelStage> stagesModelStagePaginator = new Pager<StagesModelStage>
            (
                    stagesModelStage,
                    perPage,
                    page
                );

            return Ok(stagesModelStagePaginator);
        }

        /**<summary> 
        * Function that returns all valid stagesModelStage from a organization
        * </summary>
        * <param name="idOrg"> OrganizationId </param>
        */
        [HttpGet]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Pager<StagesModelStage>>> GetStagesModelStages([FromQuery][BindRequired] int page, [FromQuery][BindRequired] int perPage)
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

            var stagesModelStage = await _context.StagesModelStages
                .Where(p => p.OrganizationId == idOrg)
                .OrderBy(p => p.Position)
                .ToListAsync();

            Pager<StagesModelStage> stagesModelStagePaginator = new Pager<StagesModelStage>
            (
                    stagesModelStage,
                    perPage,
                    page
                );

            return Ok(stagesModelStagePaginator);
        }

        /**<summary> 
         * Function that insert a new valid stagesModelStage in the database
         * </summary>
         * <param name="stagesModel"> The stagesModelStage to add </param>
         */
        [HttpPost]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<StagesModelStage>>> AddStagesModelStage(StagesModelStage stagesModelStage)
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
                StagesModelStageRequestValidator.Validate(stagesModelStage);

                stagesModelStage.OrganizationId = idOrg;

                var stageModelStageDB = await _context.StagesModelStages.FindAsync(stagesModelStage.StagesModelId, stagesModelStage.StagesId, stagesModelStage.OrganizationId);

                if (stageModelStageDB != null)
                {
                    throw new StagesModelStageArgumentException("StagesModelStage already exist!!");
                }

                var stagesOfModel = await _context.StagesModelStages
                .Where(sms => sms.StagesModelId == stagesModelStage.StagesModelId &&
                sms.OrganizationId == stagesModelStage.OrganizationId)
                .ToListAsync();

                if(!stagesOfModel.IsNullOrEmpty())
                {
                    stagesModelStage.Position = (byte)(stagesOfModel.Last().Position + 1);
                }
                else
                {
                    stagesModelStage.Position = 1;
                }

                _context.StagesModelStages.Add(stagesModelStage);
                await _context.SaveChangesAsync();

                return Ok(await _context.StagesModelStages.FindAsync(stagesModelStage.StagesModelId, stagesModelStage.StagesId, stagesModelStage.OrganizationId));
            }
            catch (DbUpdateException)
            {
                throw new StagesModelStageArgumentException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary> 
        * Function that updates an existing stagesModelStage with its new data and OrganizationId
        * </summary>
        * <param name="stagesModelStage"> The new data of stageModelStage </param>
        */
        [HttpPut]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<StagesModelStage>>> UpdateStagesModelStage(List<StagesModelStage> stagesModelStage)


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
                var stagesOfModel = await _context.StagesModelStages
                .Where(sms => sms.StagesModelId == stagesModelStage.First().StagesModelId &&
                sms.OrganizationId == stagesModelStage.First().OrganizationId)
                .ToListAsync();

                for (int i = 0; i < stagesOfModel.Count; i++)
                {
                    StagesModelStageRequestValidator.Validate(stagesOfModel[i]);

                    var result = await _context.StagesModelStages.FindAsync(stagesOfModel[i].StagesModelId, stagesOfModel[i].StagesId, stagesOfModel[i].OrganizationId);

                    if (result != null)
                    {
                        _context.StagesModelStages.Remove(stagesOfModel[i]);
                        await _context.SaveChangesAsync();
                    }
                }

                for (int i = 0; i < stagesModelStage.Count; i++)
                {
                    StagesModelStageRequestValidator.Validate(stagesModelStage[i]);

                    _context.StagesModelStages.Add(stagesModelStage[i]);
                }

                await _context.SaveChangesAsync();

                return Ok(await _context.StagesModelStages.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new StagesModelStageArgumentException("Some specified Id's are invalid! Please check again!s");
            }
        }

        /**<summary> 
         * Function that remove an existing stagesModelStage with your Id and OrganizationId
         * <summary>
         * <param name="StageModelId"> stagesModelId </param>
         * <param name="StagesId"> StagesId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpDelete]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<StagesModelStage>>> RemoveSingleStagesModelStage([FromQuery][BindRequired] string StageModelId, [FromQuery][BindRequired] string StagesId)
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
                var result = await _context.StagesModelStages.FindAsync(StageModelId, StagesId, idOrg);
                if (result == null)
                {
                    throw new StagesModelStageArgumentException("StagesModelStage not found.");
                }

                var stagesOfModel = await _context.StagesModelStages
                .Where(sms => sms.StagesModelId == StageModelId &&
                sms.OrganizationId == idOrg)
                .ToListAsync();

                if (stagesOfModel.Count == 1)
                {
                    _context.StagesModelStages.Remove(result);
                }
                else
                {
                    for (int i = stagesOfModel.Count - 1; i > stagesOfModel.IndexOf(result); i--)
                    {
                        stagesOfModel[i].Position = stagesOfModel[i - 1].Position;
                    }

                    _context.StagesModelStages.Remove(result);
                }

                await _context.SaveChangesAsync();

                return Ok(await _context.StagesModelStages.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new StagesModelStageArgumentException("You cant delete this stagesModelStage");
            }
        }

        /**<summary> 
         * Function that remove all stages of a stage model with your Id and OrganizationId
         * <summary>
         * <param name="StageModelId"> stagesModelId </param>
         * <param name="StagesId"> StagesId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpDelete("removeAllStagesModelStageByStageModelId")]
        /*[Authorize]
        [SwaggerHeader(true)]*/
        public async Task<ActionResult<List<StagesModelStage>>> RemoveAllStagesModelStageByStageModelId([FromQuery][BindRequired] string StageModelId)
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
                var stagesOfModel = await _context.StagesModelStages
                .Where(sms => sms.StagesModelId == StageModelId &&
                sms.OrganizationId == idOrg)
                .ToListAsync();

                _context.StagesModelStages.RemoveRange(stagesOfModel);

                await _context.SaveChangesAsync();

                return Ok(await _context.StagesModelStages.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new StagesModelStageArgumentException("You cant delete this stagesModelStage");
            }
        }
    }
}
