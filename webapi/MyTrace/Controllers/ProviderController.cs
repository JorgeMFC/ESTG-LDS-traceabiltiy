using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
﻿using Microsoft.AspNetCore.Authorization;
using MyTrace.Domain.Exceptions;
using MyTrace.Domain.Validations;
using MyTrace.Models;
using MyTrace.Utils;
using MyTrace.Domain;

namespace MyTrace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {

        /**<summary>
         * Context of database
         * </summary>
         */
        private readonly MyTraceContext _context;
        private readonly UserData _userGetter;
        JwtAuthenticationManager _jwtAuthenticationManager;

        /**<sumamary>
         * ProviderController Builder
         * </summary>
         */
        public ProviderController(MyTraceContext context, JwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _userGetter = new UserData(context);
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        /**<summary>
         * Function that returns all providers from a Organization
         * </summary>
         */
        [HttpGet]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Pager<Provider>>> GetProviders([FromQuery][BindRequired] int page, [FromQuery][BindRequired] int perPage, [FromQuery] string? search = null , [FromQuery] string? startDate = null, [FromQuery] string? endDate = null)
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
                throw new ProviderArgumentException("Organization Not Found!");
            }

            List<Provider> providers = new List<Provider>();

            providers = await _context.Providers
                                       .Where(u => u.OrganizationId == idOrg && u.DeletedAt == null)
                                       .Where(u => search == null || (u.Name.Contains(search) || u.Email.Contains(search)))
                                       .Where(u => startDate == null || u.CreatedAt.Date >= DateTime.Parse(startDate).Date)
                                       .Where(u => endDate == null || u.CreatedAt.Date <= DateTime.Parse(endDate).Date)
                                       .ToListAsync();

            Pager<Provider> providersPaginator = new Pager<Provider>
                (
                    providers,
                    perPage,
                    page
                );

            return Ok(providersPaginator);
        }

        /**<summary>
         * Function that returns a provider with your Id, and OrganizationId
         * </summary>
         * <param name="id"> providerId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpGet("getProviderById")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Provider>> GetProvider([FromQuery][BindRequired] string id)
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

            var provider = await _context.Providers.FindAsync(id, idOrg);
            if (provider == null)
            {
                throw new ProviderArgumentException("Provider not found.");
            }

            return Ok(provider);
        }

        /**<summary> 
         * Function that returns all providers allready deleted in database
         * </summary>
         */
        [HttpGet("getprovidersByStatus")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Provider>>> GetProvidersByStatus([FromQuery][BindRequired] int status)
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

            List<Provider> components = new List<Provider>();

            if (status == 0)
            {
                components = await _context.Providers
                    .Where(p => p.DeletedAt != null && p.OrganizationId == idOrg)
                    .ToListAsync();
            }
            else
            {
                components = await _context.Providers
                    .Where(p => p.DeletedAt == null && p.OrganizationId == idOrg)
                    .ToListAsync();
            }

            if (components.Count == 0)
            {
                throw new ProviderArgumentException("No components.");
            }

            return Ok(components);
        }

        /**<summary>
         * Function that insert a new valid provider in the database
         * </summary>
         * ´<param name="provider"> The provider to add</param>
         */
        [HttpPost]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Provider>>> AddProvider(ProviderAndComponents providerAndComponents)
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
                providerAndComponents.provider.OrganizationId = (int)idOrg;
                Provider provider = providerAndComponents.provider;
                List<Component> components = providerAndComponents.components;

                ProviderRequestValidator.Validate(provider);

                if (components == null)
                {
                    throw new ProviderArgumentException("Components list is null");
                }

                if (await _context.Providers.FindAsync(provider.Id, provider.OrganizationId) != null)
                {
                    throw new ProviderArgumentException("Provider already exist!!");
                }


                foreach (var component1 in components)
                {
                    if (component1.ProviderId == null)
                    {
                        var component = await _context.Components.FindAsync(component1.Id, component1.OrganizationId);
			            if (component != null) 
			            {
			                component.ProviderId = provider.Id;
			            }	
                    }  
                }

		        _context.Providers.Add(provider);
                await _context.SaveChangesAsync();

                return Ok(provider);
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary>
         * Function that updates an existing provider with its new data and OrganizationId
         * </summary>
         * <param name="provider"> New data for provider </param>
         */
        [HttpPut]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Provider>>> UpdateProvider(ProviderAndComponents providerAndComponents)
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
                providerAndComponents.provider.OrganizationId = (int)idOrg;
                Provider provider = providerAndComponents.provider;
                List<Component> components = providerAndComponents.components;

                ProviderRequestValidator.Validate(provider);

                if (components == null)
                {
                    throw new ProviderArgumentException("Components list is null");
                }

                var result = await _context.Providers.FindAsync(provider.Id, provider.OrganizationId);
                if (result == null || result.DeletedAt != null)
                {
                    throw new ProviderArgumentException("Provider not found.");
                }

                var componentsOld = await _context.Components.Where(p => p.ProviderId == result.Id && p.OrganizationId == result.OrganizationId).ToListAsync();

                foreach (var componentOld in componentsOld)
                {
                    componentOld.ProviderId = null;
                }

                foreach (var componentNewOld in components) 
                {
                    var newComponent = await _context.Components.FindAsync(componentNewOld.Id, componentNewOld.OrganizationId);
                    if (newComponent != null) { 
                        newComponent.ProviderId = provider.Id;
                    }
                }

                result.Name = provider.Name;
                result.Email = provider.Email;
                result.DeletedAt = provider.DeletedAt;
                result.CreatedAt = provider.CreatedAt;
                
                await _context.SaveChangesAsync();

                return Ok(await _context.Providers.FindAsync(provider.Id, provider.OrganizationId));
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary>
         * Function that activate (just update atribute deletedAt to nulll) an existing provider with your Id adn OrganizationId
         * </summary> 
         * <param name="id"> providerId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpPatch("activate")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Provider>>> ActivateProvider([FromQuery][BindRequired] string id)
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
                var result = await _context.Providers.FindAsync(id, idOrg);
                if (result == null)
                {
                    throw new ProviderArgumentException("Provider not found.");
                }

                result.DeletedAt = null;
                await _context.SaveChangesAsync();

                return Ok(await _context.Providers.FindAsync(id, idOrg));
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary>
         * Function that disable (not delete just update atribute deletedAt) an existing provider with your Id adn OrganizationId
         * </summary> 
         * <param name="id"> providerId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpPatch("disable")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Provider>>> DisableProvider([FromQuery][BindRequired] string id)
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
                var result = await _context.Providers.FindAsync(id, idOrg);
                if (result == null)
                {
                    throw new ProviderArgumentException("Provider not found.");
                }

                var components = await _context.Components
                    .Where(p => p.ProviderId == id)
                    .ToListAsync();

                foreach (var component in components)
                {
                    component.ProviderId = null;
                }

                result.DeletedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return Ok(await _context.Providers.FindAsync(id, idOrg));
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }
    }
}
