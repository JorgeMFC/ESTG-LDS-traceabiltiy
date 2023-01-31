using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MyTrace.Utils;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using System.Collections.Generic;
using MyTrace.Domain;

namespace MyTrace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentTypeController : Controller
    {
        /**<summary>
         * Context of database
         * </summary>
         */
        private readonly MyTraceContext _context;
        private readonly UserData _userGetter;
        JwtAuthenticationManager _jwtAuthenticationManager;

        /**<summary> 
         * ComponentTypeController Builder
         * </summary>
         */
        public ComponentTypeController(MyTraceContext context, JwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _userGetter = new UserData(context);
            _jwtAuthenticationManager = jwtAuthenticationManager;   
        }

        /**<summary> 
         * Function that returns all componentsTypes from a Organization
         * </summary>
         */
        [HttpGet]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<ResultComponentsType>>> GetComponentsTypes( [FromQuery][BindRequired] int page, [FromQuery][BindRequired] int perPage, [FromQuery] string? search)      
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

            List<ComponentsType> componentsTypes;

            componentsTypes = await _context.ComponentsTypes
                    .Where(p => p.OrganizationId == idOrg && p.DeletedAt == null)
                    .Where(p => search == null || p.ComponentType.Contains(search))
                    .ToListAsync();

            List<ResultComponentsType> result = new List<ResultComponentsType>();

            foreach (var componentType in componentsTypes)
            {
                var numReferences = await _context.Components.Where(p => p.ComponentsTypeId == componentType.Id && p.OrganizationId == idOrg && p.DeletedAt == null).CountAsync();

                ResultComponentsType insert = new ResultComponentsType();
                insert.componentsType = componentType;
                insert.numReferences = numReferences;

                result.Add(insert);
            }

            Pager<ResultComponentsType> componentsTypePaginator = new Pager<ResultComponentsType>
                (
                    result,
                    perPage,
                    page
                );

            return Ok(componentsTypePaginator);
        }

        /**<summary> 
         * Function that returns a componentType with your Id, and OrganizationId
         * </summary>
         * <param name="id"> componentTypeId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpGet("getComponentType")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<ComponentsType>> GetComponentType([FromQuery][BindRequired] byte id)
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

            var componentType = await _context.ComponentsTypes.FindAsync(id, idOrg);
            if (componentType == null)
            {
                throw new ComponentsTypeArgumentException("ComponentType not found.");
            }

            return Ok(componentType);
        }

        /**<summary> 
         * Function that insert a new valid componentType in the database
         * </summary>
         * <param name="componentType"> The componentType to add </param>
         */
        [HttpPost]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<ComponentsType>> AddComponentType(ComponentsType componentType)
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

            componentType.OrganizationId = idOrg;

            try
            {
                ComponentTypeRequestValidator.Validate(componentType);

                try
                {
                    var result = await _context.ComponentsTypes.Where(p => p.OrganizationId == componentType.OrganizationId && p.ComponentType == componentType.ComponentType).FirstAsync();
                
                    if (result.DeletedAt == null)
                    {
                        throw new ComponentsTypeArgumentException("This componentType already existe.");
                    }

                    result.DeletedAt = null;
                    await _context.SaveChangesAsync();
                                      
                }
                catch (InvalidOperationException) {
                    _context.ComponentsTypes.Add(componentType);
                    await _context.SaveChangesAsync();
                }

                return Ok(await _context.ComponentsTypes.Where(p => p.OrganizationId == componentType.OrganizationId).ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new ComponentsTypeArgumentException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary> 
         * Function that updates an existing componentType with its new data and OrganizationId
         * </summary>
         * <param name="componentType"> The new data of componentType </param>
         */
        [HttpPut]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<ComponentsType>>> UpdateComponentType(ComponentsType componentType)
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

            componentType.OrganizationId = idOrg;

            try
            {
                ComponentTypeRequestValidator.Validate(componentType);

                var result = await _context.ComponentsTypes.FindAsync(componentType.Id, componentType.OrganizationId);
                if (result == null)
                {
                    throw new ComponentsTypeArgumentException("ComponentType not found.");
                }

                result.OrganizationId = componentType.OrganizationId;
                result.ComponentType = componentType.ComponentType;

                await _context.SaveChangesAsync();

                return Ok(await _context.ComponentsTypes.FindAsync(componentType.Id, componentType.OrganizationId));
            }
            catch (DbUpdateException)
            {
                throw new ComponentsTypeArgumentException("Some specified Id's are invalid! Please check again!s");
            }
        }

        /**<summary> 
         * Function that remove an existing componentType with your Id adn OrganizationId
         * <summary>
         * <param name="id"> componentTypeId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpDelete]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<ComponentsType>> RemoveComponentType([FromQuery][BindRequired] byte id)
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
                var result = await _context.ComponentsTypes.FindAsync(id, idOrg);
                if (result == null)
                {
                    throw new ComponentsTypeArgumentException("ComponentType not found.");
                }

                if (result == null)
                {
                    throw new ComponentsTypeArgumentException("This ComponentType already removed.");
                }

                result.DeletedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return Ok(await _context.ComponentsTypes.FindAsync(id, idOrg));
            }
            catch(DbUpdateException)
            {
                throw new ComponentsTypeArgumentException("You cant delete this componentType, maybe exist componets with this type");
            }
        }

    }
}
