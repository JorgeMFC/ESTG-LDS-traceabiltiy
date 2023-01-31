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
    public class ColorsSizesController : Controller
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
        public ColorsSizesController(MyTraceContext context, JwtAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _userGetter = new UserData(context);
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        /**<summary> 
         * Function that returns all colors from organization
         * </summary>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpGet("getColors")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Pager<Color>>> GetColor([FromQuery][BindRequired] int page, [FromQuery][BindRequired] int perPage)    
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

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

            var colors = await _context.Colors
                .Where(p => p.OrganizationId == idOrg)
                .ToListAsync();

            Pager<Color> colorsPaginator = new Pager<Color>
                (
                    colors,
                    perPage,
                    page
                );

            return Ok(colorsPaginator);
        }

        /**<summary> 
         * Function that returns a color with your Id, and OrganizationId
         * </summary>
         * <param name="id"> colorId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpGet("getColor")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Color>> GetColor([FromQuery][BindRequired] string id)
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

            var color = await _context.Colors.FindAsync(id, idOrg);
            if (color == null)
            {
                throw new ModelArgumentException("Color not found.");
            }

            return Ok(color);
        }

        /**<summary> 
        * Function that insert a new valid color in the database
        * </summary>
        * <param name="color"> The color to add </param>
        */
        [HttpPost("addColor")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Color>>> AddColor(Color color)
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
                throw new LotArgumentException("Organization Not Found!");
            }

            color.OrganizationId = (int)idOrg;

            try
            {
                ColorRequestValidator.Validate(color);

                if (await _context.Colors.FindAsync(color.Id, color.OrganizationId) != null)
                {
                    throw new ModelArgumentException("Color allready exist!!");
                }

                _context.Colors.Add(color);
                await _context.SaveChangesAsync();

                return Ok(await _context.Colors.FindAsync(color.Id, color.OrganizationId));
            }
            catch (DbUpdateException)
            {
                throw new ModelArgumentException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary> 
         * Function that returns a size list of a model
         * </summary>
         * <param name="id"> sizeId </param>
         */
        [HttpGet("getColorsByModelId")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Size>>> GetColorsByModelId([FromQuery][BindRequired] string modelId)
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

            var colorsIds = await _context.ModelsSizesColors
                   .Where(msc => msc.ModelId == modelId && msc.OrganizationId == idOrg)
                   .ToListAsync();

            List<Color> colors = new List<Color>();

            for (int i = 0; i < colorsIds.Count; i++)
            {
                var colorData = await _context.Colors
                   .Where(c => c.Id == colorsIds[i].ColorId)
                   .SingleAsync();

                colors.Add(colorData);
            }

            return Ok(colors);
        }

        /**<summary> 
         * Function that remove an existing color with your Id and OrganizationId
         * <summary>
         * <param name="id"> colorId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpDelete("removeColor")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Color>>> RemoveColor([FromQuery][BindRequired] string id)
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
                var result = await _context.Colors.FindAsync(id, idOrg);
                if (result == null)
                {
                    throw new ModelArgumentException("Color not found.");
                }

                _context.Colors.Remove(result);
                await _context.SaveChangesAsync();

                return Ok(await _context.Colors.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new ModelArgumentException("You cant delete this color, maybe exist models with this color");
            }
        }

        /**<summary> 
         * Function that returns all sizes from organization
         * </summary>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpGet("getSizes")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Pager<Size>>> GetSizes([FromQuery][BindRequired] int page, [FromQuery][BindRequired] int perPage)
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

            var sizes = await _context.Sizes
                .Where(p => p.OrganizationId == idOrg)
                .ToListAsync();

            Pager<Size> sizesPaginator = new Pager<Size>
                (
                    sizes,
                    perPage,
                    page
                );

            return Ok(sizesPaginator);
        }

        /**<summary> 
         * Function that returns a size with your Id, and OrganizationId
         * </summary>
         * <param name="id"> sizeId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpGet("getSize")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Color>> GetSize([FromQuery][BindRequired] string id)
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

            var size = await _context.Colors.FindAsync(id, idOrg);
            if (size == null)
            {
                throw new ModelArgumentException("Size not found.");
            }

            return Ok(size);
        }

        /**<summary> 
         * Function that returns a size list of a model
         * </summary>
         * <param name="id"> sizeId </param>
         */
        [HttpGet("getSizesByModelId")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Size>>> GetSizesByModelId([FromQuery][BindRequired] string modelId)
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

            var sizesIds = await _context.ModelsSizesColors
                   .Where(msc => msc.ModelId == modelId && msc.OrganizationId == idOrg)
                   .ToListAsync();

            List<Size> sizes = new List<Size>();

            for (int i = 0; i < sizesIds.Count; i++)
            {
                var sizeData = await _context.Sizes
                   .Where(s => s.Id == sizesIds[i].SizeId)
                   .SingleAsync();

                sizes.Add(sizeData);
            }

            return Ok(sizes);
        }

        /**<summary> 
         * Function that insert a new valid size in the database
         * </summary>
         * <param name="size"> The size to add </param>
         */
        [HttpPost("addSize")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Size>>> AddSize(Size size)
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
                throw new LotArgumentException("Organization Not Found!");
            }

            size.OrganizationId = (int)idOrg;

            try
            {
                SizeRequestValidator.Validate(size);

                if (await _context.Sizes.FindAsync(size.Id, size.OrganizationId) != null)
                {
                    throw new ModelArgumentException("Size allready exist!!");
                }

                _context.Sizes.Add(size);
                await _context.SaveChangesAsync();

                return Ok(await _context.Sizes.FindAsync(size.Id, size.OrganizationId));
            }
            catch (DbUpdateException)
            {
                throw new ModelArgumentException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary> 
         * Function that remove an existing size with your Id and OrganizationId
         * <summary>
         * <param name="id"> sizeId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpDelete("removeSize")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Size>>> RemoveSize([FromQuery][BindRequired] string id)  
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
                var result = await _context.Sizes.FindAsync(id, idOrg);
                if (result == null)
                {
                    throw new ModelArgumentException("Size not found.");
                }

                _context.Sizes.Remove(result);
                await _context.SaveChangesAsync();

                return Ok(await _context.Sizes.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new ModelArgumentException("You cant delete this size, maybe exist models with this size");
            }
        } 
    }
}
