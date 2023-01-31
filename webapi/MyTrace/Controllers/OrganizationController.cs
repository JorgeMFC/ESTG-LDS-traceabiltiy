using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MyTrace.Domain;
using MyTrace.Models;
using MyTrace.Utils;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;

namespace MyTrace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly MyTraceContext _context;
        private readonly JwtAuthenticationManager _jwtAuthenticationManager;
        private readonly UserData _userGetter;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public OrganizationController(MyTraceContext context, JwtAuthenticationManager jwtAuthenticationManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _webHostEnvironment = webHostEnvironment;
            _userGetter = new UserData(context);
        }

        /**<summary>
         * Get all organizations
         * </summary>
         */
        [HttpGet]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Pager<OrganizationResponse>>> GetOrganizations([FromQuery][BindRequired] int page, [FromQuery][BindRequired] int perPage, [FromQuery] string? search,[FromQuery] string? owner)
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

            var userType = await _context.UsersTypes.FindAsync(user1.UserTypeId);

            if (userType != null && userType.UserType != "Admin")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var organizations = await _context.Organizations
            .Where(p => search == null || p.Name.Contains(search) || p.Email.Contains(search))
            .ToListAsync();

            List<OrganizationResponse> organizationResponses = new List<OrganizationResponse>();

            foreach (var organization in organizations)
            {
                try
                {
                    var user = await _context.Users
                                        .Where(p => p.OrganizationId == organization.Id &&
                                                    p.UserTypeId == 1).SingleAsync();

                    OrganizationResponse organizationResponse = new OrganizationResponse();

                    organizationResponse.Organization = organization;
                    organizationResponse.User = user;

                    organizationResponses.Add(organizationResponse);

                } catch (InvalidOperationException)
                {
                    throw new OrganizationArgumentException("Dont exist a owner to this organization!");
                }              
            }

            organizationResponses = organizationResponses.Where(p => owner == null || p.User.Name.Contains(owner)).ToList();

            Pager<OrganizationResponse> organizationPaginator = new Pager<OrganizationResponse>
                (
                    organizationResponses,
                    perPage,
                    page
                );

            return Ok(organizationPaginator);
        }

        /**<summary>
         * Get an organization by Id
         * </summary>
         * <param name="idOrg">the organization id</param>
         */
        [HttpGet("getOrganization")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Organization>> GetOrganization()
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

            if (organization == null)
            {
                throw new OrganizationArgumentException("Organization not found!");
            }

            return Ok(organization);
        }

        /**<summary>
         * Add new organization
         * </summary>
         * <param name="organization">the organization to be added</param>
         */
        [HttpPost]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Organization>>> AddOrganization([Required][FromForm] OrganizationRequest organizationRequest)
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

            if (userType != null && user.OrganizationId != null && userType.UserType == "User")
            {
                throw new AuthenticationArgumentException("You already have an Organization");
            }

            if (userType != null && userType.UserType != "Admin" && userType.UserType != "User")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            try
            {
                var organization = organizationRequest.Organization;

                OrganizationRequestValidator.Validate(organization);

                try { 
                    if (await _context.Organizations.Where(p => p.WalletAddress == organization.WalletAddress).FirstAsync() != null)
                    {
                        throw new OrganizationArgumentException("Organization already exists!");
                    }
                }catch (InvalidOperationException){ }

                if (organizationRequest.organizationLogo != null)
                {
                    Images images = new Images(_webHostEnvironment);
                    string? urlImage = await images.saveImage(organizationRequest.organizationLogo);

                    if (urlImage == null)
                    {
                        throw new UserArgumentException("Invalid image");
                    }

                    organization.Logo = urlImage;
                }

                if (organizationRequest.organizationPhoto != null)
                {
                    Images images = new Images(_webHostEnvironment);
                    string? urlImage = await images.saveImage(organizationRequest.organizationPhoto);

                    if (urlImage == null)
                    {
                        throw new UserArgumentException("Invalid image");
                    }

                    organization.Photo = urlImage;
                }
          
                _context.Organizations.Add(organization);
                await _context.SaveChangesAsync();

                if (userType != null && userType.UserType == "User")
                {
                    user.UserTypeId = 1;
                    var org = await _context.Organizations.Where(p => p.WalletAddress == organization.WalletAddress).SingleAsync();
                    user.OrganizationId = org.Id;
                }

                await _context.SaveChangesAsync();

                return Ok(await _context.Organizations.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /** <summary>
         * Update an existant organization
         * </summary>
         *<param name="organization">the organization to be updated</param> 
         */
        [HttpPut]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Organization>>> UpdateOrganization([Required][FromForm] OrganizationRequest organizationRequest)
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

            if (userType != null && userType.UserType != "Owner")
            {
                throw new AuthenticationArgumentException("Invalid permissions");
            }

            var idOrg = (int)user.OrganizationId;
            var organization1 = await _context.Organizations.FindAsync(idOrg);

            if (organization1 == null)
            {
                throw new ModelArgumentException("Organization Not Found!");
            }

            try
            {
                var organization = organizationRequest.Organization;
                organization.Id = idOrg;

                OrganizationRequestValidator.Validate(organization);

                var result = await _context.Organizations.FindAsync(organization.Id);

                if (result == null)
                {
                    return NotFound("Organization not found!");
                }

                result.Id = organization.Id;
                result.Name = organization.Name;
                result.Email = organization.Email;
                result.WalletAddress = organization.WalletAddress;  
                result.RegexIdOrganizationsAddresses = organization.RegexIdOrganizationsAddresses;
                result.RegexIdLots = organization.RegexIdLots;  
                result.RegexIdColors = organization.RegexIdColors;
                result.RegexIdCoponents = organization.RegexIdCoponents;
                result.RegexComponentsType = organization.RegexComponentsType;
                result.RegexIdModels= organization.RegexIdModels;
                result.RegexIdProviders = organization.RegexIdProviders;
                result.RegexIdSizes = organization.RegexIdSizes;
                result.RegexIdStates = organization.RegexIdStates;
                result.RegexIdStatesModel = organization.RegexIdStatesModel;
                result.RegexIdStatesType = organization.RegexIdStatesType;

                if (organizationRequest.organizationLogo != null)
                {
                    Images images = new Images(_webHostEnvironment);
                    string? urlImage = await images.saveImage(organizationRequest.organizationLogo);

                    if (urlImage == null)
                    {
                        throw new UserArgumentException("Invalid image");
                    }

                    organization.Logo = urlImage;
                }

                if (organizationRequest.organizationPhoto != null)
                {
                    Images images = new Images(_webHostEnvironment);
                    string? urlImage = await images.saveImage(organizationRequest.organizationPhoto);

                    if (urlImage == null)
                    {
                        throw new UserArgumentException("Invalid image");
                    }

                    organization.Photo = urlImage;
                }

                await _context.SaveChangesAsync();

                return Ok(await _context.Organizations.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }
    }
}
