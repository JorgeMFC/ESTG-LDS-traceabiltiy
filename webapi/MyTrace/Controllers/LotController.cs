using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyTrace.Blockchain;
using MyTrace.Models;
using MyTrace.Utils;
using Nethereum.Model;
using System.Globalization;
using MyTrace.Domain;
using System.ComponentModel.DataAnnotations;

namespace MyTrace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LotController : ControllerBase
    {
        private readonly MyTraceContext _context;
        private readonly UserData _userGetter;
        private readonly PinataEnv _pinataEnv;
        private readonly CryptocurrencyBrokerageEnv _cryptocurrencyBrokerageEnv;
        private readonly IWebHostEnvironment _webHostEnvironment;
        JwtAuthenticationManager _jwtAuthenticationManager;

        public LotController(
            MyTraceContext context,
            IOptions<CryptocurrencyBrokerageEnv> cryptocurrencyBrokerageEnv,
            IOptions<PinataEnv>  pinataEnv,
            IWebHostEnvironment webHostEnvironment,
            JwtAuthenticationManager jwtAuthenticationManager
        )
        {
            _context = context;
            _userGetter = new UserData(context);
            _pinataEnv = pinataEnv.Value;
            _cryptocurrencyBrokerageEnv = cryptocurrencyBrokerageEnv.Value;
            _webHostEnvironment = webHostEnvironment;
            _jwtAuthenticationManager = jwtAuthenticationManager;   
        }

        /** <summary>
         * Get lots by organization 
         * </summary>
         * <param name="idOrg">The id of organization</param>
         */
        [HttpGet]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Pager<LotResponse>>> GetLots([FromQuery][BindRequired] int page, [FromQuery][BindRequired] int perPage, [FromQuery] string? search, [FromQuery] string? stageId)
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

            var lots = await _context.Lots
                    .Where(l => l.OrganizationId == idOrg)
                    .Where(l => stageId == null || l.StagesModelId == stageId)
                    .ToListAsync();

            var models = await _context.Models
                .Where(p => p.OrganizationId == idOrg)
                .Where(p => search == null || p.Name.Contains(search))
                .ToListAsync();

            List<Lot> lots1 = lots
                .Where(lot => models.Any(model => model.Id == lot.ModelId)).ToList();

            List<LotResponse> lotResponses = new List<LotResponse>();

            foreach (var lot in lots1)
            {
                LotResponse lotResponse = new LotResponse();

                lotResponse.Lot = lot;

                var model = await _context.Models.FindAsync(lot.ModelId, idOrg);
                if (model != null) lotResponse.NomeModelo = model.Name;

                var color = await _context.Colors.FindAsync(lot.ModelColorId, idOrg);
                if (color != null) lotResponse.NomeCor = color.Color1;

                var size = await _context.Sizes.FindAsync(lot.ModelSizeId, idOrg);
                if (size != null) lotResponse.NomeTamanho = size.Size1;

                var client = await _context.Clients.FindAsync(lot.ClientId, idOrg);
                if (client != null) lotResponse.NomeCliente = client.Name;

                var lotStages = await _context.LotsStages
                    .Where(ls => ls.LotId == lot.Id &&
                    ls.OrganizationId == idOrg &&
                    ls.EndDate == null).ToListAsync();

                if (lotStages.Count == 0)
                {
                    lotResponse.NomeEtapa = "No Stage";
                } else
                {
                    var stage = await _context.Stages.FindAsync(lotStages.First().StageId, idOrg);
                    if (stage != null) lotResponse.NomeEtapa = stage.StageName;
                }

                lotResponses.Add(lotResponse);
            }

            Pager<LotResponse> lotsPaginator = new Pager<LotResponse>
                (
                    lotResponses,
                    perPage,
                    page
                );
            
            return Ok(lotsPaginator);
        }

        /** <summary>
         * Get lots by clientId
         * </summary>
         * <param name="clientId">The id of client</param>
         */
        [HttpGet("getLotsByClientId")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Pager<Lot>>> GetLotsByClientId([FromQuery][BindRequired] string clientId, [FromQuery][BindRequired] int page, [FromQuery][BindRequired] int perPage, [FromQuery] string? search, [FromQuery] string? stageId)
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

            var client = await _context.Clients.FindAsync(clientId, idOrg);


            if (client == null)
            {
                throw new LotArgumentException("Client Not Found!");
            }

            var lots = await _context.Lots
                    .Where(l => l.OrganizationId == idOrg && l.ClientId == clientId)
                    .Where(l => stageId == null || l.StagesModelId == stageId)
                    .ToListAsync();

            var models = await _context.Models
                .Where(p => p.OrganizationId == idOrg)
                .Where(p => search == null || p.Name.Contains(search))
                .ToListAsync();

            List<Lot> lots1 = lots
                .Where(lot => models.Any(model => model.Id == lot.ModelId)).ToList();

            Pager<Lot> lotsPaginator = new Pager<Lot>
                (
                    lots1,
                    perPage,
                    page
                );

            return Ok(lotsPaginator);
        }


        /** <summary>
         * Get a lot by id
         * </summary>
         * <param name="id">The id of organization</param>
         */
        [HttpGet("getLotById")]
        [AllowAnonymous]
        public async Task<ActionResult<Lot>> GetLotById([FromQuery][BindRequired] string id)
        {
            try
            {
                var lot = await _context.Lots.Where(l => l.Id == id).SingleAsync();

                return Ok(lot);
            } catch(InvalidOperationException)
            {
                throw new LotArgumentException("Lot not found!");
            }
        }

        private async Task ValidateLot(Lot lot)
        {
            try
            {
                LotRequestValidator.Validate(lot);

                Lot? lotData = await _context.Lots.FindAsync(lot.Id, lot.OrganizationId);

                if (lotData != null)
                {
                    throw new LotArgumentException("Lot already exists!");
                }

                if (await _context.Models.FindAsync(lot.ModelId, lot.OrganizationId) == null)
                {
                    throw new LotArgumentException("The selected model doesn't exists in specified organization!");
                }

                if (await _context.StagesModels.FindAsync(lot.StagesModelId, lot.OrganizationId) == null)
                {
                    throw new LotArgumentException("The selected stage model doesn't exists in specified organization!");
                }
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        [HttpPost("CreateNFTLot")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<string>> CreateNFTLot(Lot lot)
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

            try
            {
                await ValidateLot(lot);

                Organization? organization = await _context.Organizations.FindAsync(lot.OrganizationId);

                if (organization == null)
                {
                    throw new LotArgumentException("Not exist organization!");
                }

                SmartContract smartContract = new SmartContract(_cryptocurrencyBrokerageEnv, _pinataEnv, _webHostEnvironment);
                string? hash = await smartContract.CreateNFTLot(lot, organization);

                if (hash == null)
                {
                    throw new LotArgumentException("Error creating nft");
                }

                return Ok(hash);
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /** <summary>
         * Add a new lot 
         * </summary>
         * <param name="lot">The lot to be added</param>
         */
        [HttpPost]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Lot>>> AddLot(Lot lot)
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

            lot.OrganizationId = idOrg;

            try
            {
                await ValidateLot(lot);

                SmartContract smartContract = new SmartContract(_cryptocurrencyBrokerageEnv, _pinataEnv, _webHostEnvironment);
                if (!smartContract.validateHashTransaction(lot.Hash).Result)
                {
                    throw new AuthenticationArgumentException("Invalid hash");
                }

                _context.Lots.Add(lot);
                await _context.SaveChangesAsync();

                return Ok(await _context.Lots.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

       
        [HttpPut("cancelLot")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Lot>>> CancelLot([FromQuery][BindRequired] string lotId)
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
                var result = await _context.Lots.FindAsync(lotId, idOrg);

                if (result == null)
                {
                    throw new LotArgumentException("Lot Not Found!");
                }

                if (result.CanceledAt != null)
                {
                    throw new LotArgumentException("Lot has already been canceled!");
                }

                result.CanceledAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return Ok(await _context.Lots.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /** <summary>
         * Get all stages of given lot
         * </summary>
         * <param name="lotId">The lot id</param>
         * <param name="organizationId">The id of lot organization</param>
         */
        [HttpGet("getLotStages")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Lot>> getLotStages([FromQuery][BindRequired] string lotId)
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

            var lot = await _context.Lots.FindAsync(lotId, idOrg);


            if (lot == null)
            {
                throw new LotArgumentException("The specified lot was not found!");
            }

            var lotStages = _context.LotsStages
                    .Where(ls => ls.LotId == lotId && ls.OrganizationId == idOrg).ToListAsync();

            
            if (lotStages == null)
            {
                throw new LotArgumentException("Any stage was found for this lot!");
            }

            return Ok(lotStages);
        }

        /** <summary>
         * Get the actual stage of lot
         * </summary>
         * <param name="lotId">The lot id</param>
         * <param name="organizationId">The id of lot organization</param>
         */
        [HttpGet("getActualLotStage")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Lot>> getActualLotStage([FromQuery] string lotId)
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

            var lot = await _context.Lots.FindAsync(lotId, idOrg);

            if (lot == null)
            {
                throw new LotArgumentException("The specified lot was not found!");
            }

            var lotStages = await _context.LotsStages
                    .Where(ls => ls.LotId == lotId && 
                    ls.OrganizationId == idOrg && 
                    ls.EndDate == null).ToListAsync();


            if (lotStages == null)
            {
                throw new LotArgumentException("Any stage was found for this lot!");
            }

            return Ok(lotStages);
        }

        private async Task<LotsStage> getNextState(Lot lot)
        {
            if (lot == null)
            {
                throw new LotArgumentException("The specified lot was not found!");
            }

            var stagesOfModel = await _context.StagesModelStages
                .Where(sms => sms.StagesModelId == lot.StagesModelId &&
                sms.OrganizationId == lot.OrganizationId)
                .OrderBy(sms => sms.Position)
                .ToListAsync();

            var lotCurrentStages = await _context.LotsStages
                    .Where(ls => ls.LotId == lot.Id &&
                    ls.OrganizationId == lot.OrganizationId)
                    .ToListAsync();


            if (lotCurrentStages.IsNullOrEmpty())
            {
                var firstLotStage = new LotsStage();

                firstLotStage.LotId = lot.Id;
                firstLotStage.OrganizationId = lot.OrganizationId;
                firstLotStage.StageId = stagesOfModel.First().StagesId;
                firstLotStage.Hash = null;
                firstLotStage.StartDate = DateTime.Now;
                firstLotStage.EndDate = null;

                return firstLotStage;

            }
            else
            {
                try
                {
                    var currentLotStage = await _context.LotsStages
                    .Where(ls => ls.LotId == lot.Id &&
                    ls.OrganizationId == lot.OrganizationId &&
                    ls.EndDate == null).SingleAsync();

                    currentLotStage.EndDate = DateTime.Now;

                    if (stagesOfModel.Last().StagesId != currentLotStage.StageId)
                    {
                        var nextLotStage = new LotsStage();

                        var stagePositionInModel = await _context.StagesModelStages.FindAsync(lot.StagesModelId, currentLotStage.StageId, lot.OrganizationId);

                        var nextStageId = await _context.StagesModelStages
                            .Where(sms => sms.StagesModelId == lot.StagesModelId &&
                            sms.OrganizationId == lot.OrganizationId &&
                            sms.Position == (stagePositionInModel.Position + 1)).SingleAsync();

                        nextLotStage.LotId = lot.Id;
                        nextLotStage.OrganizationId = lot.OrganizationId;
                        nextLotStage.StageId = nextStageId.StagesId;
                        nextLotStage.Hash = null;
                        nextLotStage.StartDate = DateTime.Now;
                        nextLotStage.EndDate = null;

                        return nextLotStage;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    throw new LotArgumentException("This lot is already finished!");
                }
                throw new AuthenticationArgumentException("Invalid data");

            }
        }

        [HttpPost("CreateNFTNextLotStage")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<string>> CreateNFTNextLotStage([FromQuery][BindRequired] string lotId)
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

            if (user.OrganizationId == null)
            {
                throw new AuthenticationArgumentException("This user dont have organization");
            }

            int idOrg = (int)user.OrganizationId;

            Lot lot = await _context.Lots.FindAsync(lotId, idOrg);

            LotsStage nextLotStage = getNextState(lot).Result;

            Stage? stage = await _context.Stages.FindAsync(nextLotStage.StageId, idOrg);

            if (stage == null)
            {
                throw new LotArgumentException("Not exist stage!");
            }

            SmartContract smartContract = new SmartContract(_cryptocurrencyBrokerageEnv, _pinataEnv, _webHostEnvironment);
            string? hash = await smartContract.CreateNFTState(lot, stage);

            if (hash == null)
            {
                throw new LotArgumentException("Error creating and publishing NFT!");
            }

            return hash;


        }

        [HttpPatch("advanceToNextLotStage")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Lot>> AdvanceToNextLotStage([FromQuery][BindRequired] string lotId, [FromBody][Required] string hash)
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

            if (user.OrganizationId == null)
            {
                throw new AuthenticationArgumentException("This user dont have organization");
            }

            try
            {
                SmartContract smartContract = new SmartContract(_cryptocurrencyBrokerageEnv, _pinataEnv, _webHostEnvironment);
                if (!smartContract.validateHashTransaction(hash).Result)
                {
                    throw new AuthenticationArgumentException("Invalid hash");
                }

                int idOrg = (int)user.OrganizationId;

                Lot lot = await _context.Lots.FindAsync(lotId, idOrg);

                LotsStage nextLotStage = getNextState(lot).Result;

                _context.LotsStages.Add(nextLotStage);

                _context.SaveChanges();
            } catch (DbUpdateException)
            {
                throw new LotArgumentException("This hash already exist!!");
            }
            

            return Ok("Lot is moved for next stage.");
        }
        
    }
}
