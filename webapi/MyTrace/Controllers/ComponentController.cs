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
    public class ComponentController : Controller
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
        public ComponentController(MyTraceContext context, JwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _userGetter = new UserData(context);
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        /**<summary> 
         * Function that returns all valid components from a organization
         * </summary>
         */
        [HttpGet]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Pager<Component>>> GetComponents([FromQuery][BindRequired] int page, [FromQuery][BindRequired] int perPage)        
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
                throw new ComponentArgumentException("Organization Not Found!");
            }

            var components = await _context.Components
                .Where(p => p.DeletedAt == null &&
                            p.OrganizationId == idOrg)
            .ToListAsync();

            Pager<Component> componentsPaginator = new Pager<Component>
                (
                    components,
                    perPage,
                    page
                );

            return Ok(componentsPaginator);
        }

        /**<summary> 
         * Function that returns a component with your Id, and OrganizationId
         * </summary>
         * <param name="id"> componentId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpGet("getComponentById")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Component>> GetComponent([FromQuery][BindRequired] string id)
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

            var component = await _context.Components.FindAsync(id, idOrg); 
            if (component == null)
            {
                throw new ComponentArgumentException("Component not found.");
            }

            return Ok(component);
        }

        /**<summary> 
         * Function that returns all valid components with the componentTypeId
         * </summary>
         * <param name="componentTypeId"> componentTypeId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpGet("getComponentsByTypeId")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Component>>> GetComponentsByTypeId([FromQuery][BindRequired] byte componentTypeId)
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

            var components = await _context.Components
                .Where(p => p.DeletedAt == null &&
                            p.ComponentsTypeId == componentTypeId &&
                            p.OrganizationId == idOrg)
                .ToListAsync();

            return Ok(components);
        }

        /**<summary> 
         * Function that returns all valid components with the componentTypeId
         * </summary>
         * <param name="componentTypeId"> componentTypeId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpGet("getComponentsByModelId")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Component>>> GetComponentsByModelId([FromQuery][BindRequired] string modelId)
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

            var componentsIds = await _context.ModelsComponents
                   .Where(mc => mc.ModelId == modelId && mc.OrganizationId == idOrg)
                   .ToListAsync();

            List<Component> components = new List<Component>();

            for (int i = 0; i < componentsIds.Count; i++)
            {
                var componentData = await _context.Components
                   .Where(c => c.Id == componentsIds[i].ComponentsId)
                   .SingleAsync();

                components.Add(componentData);
            }

            return Ok(componentsIds);
        }

        /**<summary> 
         * Function that returns all components allready deleted in database
         * </summary>
         */
        [HttpGet("getComponentsByStatus")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Component>>> GetComponentsByStatus([FromQuery][BindRequired] int status)
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

            List<Component> components = new List<Component>();

            if (status == 0)
            {
                components = await _context.Components
                    .Where(p => p.DeletedAt != null && p.OrganizationId == idOrg)
                    .ToListAsync();
            } else
            {
                components = await _context.Components
                    .Where(p => p.DeletedAt == null && p.OrganizationId == idOrg)
                    .ToListAsync();
            }

            return Ok(components);
        }

        /**<summary> 
         * Function that return all valid components without provider
         * </summary>
         */
        [HttpGet("getComponentsWithoutProvider")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Component>>> GetComponentsWithoutProvider()
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

            var components = await _context.Components
                    .Where(p => p.DeletedAt == null &&
                                p.ProviderId == null &&
                                p.OrganizationId == idOrg)
                    .ToListAsync();

            return Ok(components);
        }

        /**<summary> 
         * Function that returns all valid components by providerId
         * </summary>
         * <param name="providerId"> providerId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpGet("getComponentsByproviderId")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Provider>>> GetComponentsByProviderId([FromQuery][BindRequired] string providerId)
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

            var components = await _context.Components
                    .Where(p => p.DeletedAt == null && 
                                p.ProviderId == providerId &&
                                p.OrganizationId == idOrg)
                    .ToListAsync();

            return Ok(components);
        }

        /**<summary> 
         * Function that insert a new valid component in the database
         * </summary>
         * <param name="component"> The component to add </param>
         */
        [HttpPost]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Component>>> AddComponent(Component component)
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
                throw new LotArgumentException("Organization Not Found!");
            }

            component.OrganizationId = idOrg;

            try
            {
                ComponentRequestValidator.Validate(component);

                var result = await _context.Components.FindAsync(component.Id, component.OrganizationId);

                if (result != null && result.DeletedAt == null)
                {
                    throw new ComponentArgumentException("This component already existe.");
                }

                if (result != null &&
                    result.Id == component.Id &&
                    result.OrganizationId == component.OrganizationId &&
                    result.ComponentsTypeId == component.ComponentsTypeId &&
                    result.DeletedAt != null)
                {
                    result.DeletedAt = null;
                    await _context.SaveChangesAsync();
                } 
                else if (result == null)
                {
                    _context.Components.Add(component);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ComponentArgumentException("This component already existe but is componentType is diferent.");
                }

                return Ok(await _context.Components.FindAsync(component.Id, component.OrganizationId));
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary>
         * Function that updates an existing component with its new data and OrganizationId
         * </summary>
         * <param name="component"> The new data od component </param>
         */
        [HttpPut]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Component>>> UpdateComponent(Component component)
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
                throw new LotArgumentException("Organization Not Found!");
            }

            try
            {
                ComponentRequestValidator.Validate(component);

                var result = await _context.Components.FindAsync(component.Id, component.OrganizationId);
                if (result == null || result.DeletedAt != null)
                {
                    throw new ComponentArgumentException("Component not found.");
                }

                result.ProviderId = component.ProviderId;
                result.ComponentsTypeId = component.ComponentsTypeId;
                result.DeletedAt = component.DeletedAt;

                await _context.SaveChangesAsync();

                return Ok(await _context.Components.FindAsync(component.Id, component.OrganizationId));
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary> 
         * Function that activate (just update atribute deletedAt to nulll) an existing component with your Id adn OrganizationId
         * </summary>
         * <param name="id"> componentId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpPatch("activate")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Component>>> ActivateComponent([FromQuery][BindRequired] string id)
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
                var result = await _context.Components.FindAsync(id, idOrg);
                if (result == null)
                {
                    throw new ComponentArgumentException("Component not found.");
                }

                if (result.DeletedAt == null)
                {
                    throw new ComponentArgumentException("This component is already active.");
                }

                result.DeletedAt = null;
                await _context.SaveChangesAsync();

                return Ok(await _context.Components.FindAsync(id, idOrg));
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary> 
         * Function that remove (not delete just update atribute deletedAt) an existing component with your Id adn OrganizationId
         * </summary>
         * <param name="id"> componentId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpPatch("disable")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Component>>> DisableComponent([FromQuery][BindRequired] string id)
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
                var result = await _context.Components.FindAsync(id, idOrg);
                if (result == null)
                {
                    throw new ComponentArgumentException("Component not found.");
                }

                if (result.DeletedAt != null)
                {
                    throw new ComponentArgumentException("This component is already disable.");
                }

                result.DeletedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return Ok(await _context.Components.FindAsync(id, idOrg));
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }
    }
}
