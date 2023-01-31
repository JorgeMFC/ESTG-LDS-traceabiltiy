using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.WebEncoders.Testing;
using Microsoft.IdentityModel.Tokens;
using MyTrace.Models;
using MyTrace.Utils;
using MyTrace.Domain;
using Microsoft.AspNetCore.Hosting;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using System.ComponentModel.DataAnnotations;

namespace MyTrace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelController : Controller
    {
        /**<summary> 
         * Context of database
         * </summary>
         */
        private readonly MyTraceContext _context;
        private readonly UserData _userGetter;
        private readonly JwtAuthenticationManager _jwtAuthenticationManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /**<summary> 
         * ModelController Builder 
         * </summary>
         */
        public ModelController(MyTraceContext context, JwtAuthenticationManager jwtAuthenticationManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userGetter = new UserData(context);
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _webHostEnvironment = webHostEnvironment;
        }

        /**<summary> 
         * Function that returns all valid models from a Organization
         * </summary>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpGet]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Pager<ModelResponse>>> GetModels([FromQuery][BindRequired] int page, [FromQuery][BindRequired] int perPage, [FromQuery] string? search, [FromQuery] ColorsAndSizesRequest? colorsAndSizes = null)
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

            List<Model> models = new List<Model>();

            models = await _context.Models
                                .Where(p => p.OrganizationId == idOrg && p.DeletedAt == null)
                                .Where(p => search == null || p.Name.Contains(search))
                                .ToListAsync();

            List<ModelResponse> modelResponses = new List<ModelResponse>();

            foreach (var model in models)
            {
                ModelResponse modelResponse = new ModelResponse();
                List<Color> colors = new List<Color>();
                List<Size> sizes = new List<Size>();

                var modelsSizesColor = await _context.ModelsSizesColors.Where(p => p.ModelId == model.Id && p.OrganizationId == idOrg).ToListAsync();

                modelResponse.Model = model;

                foreach (var modelSizeColor in modelsSizesColor)
                {
                    var color = await _context.Colors.FindAsync(modelSizeColor.ColorId, idOrg);
                    var size = await _context.Sizes.FindAsync(modelSizeColor.SizeId, idOrg);

                    if (color != null && size != null)
                    {
                        if (!colors.Contains(color)) 
                        {
                            colors.Add(color);
                        }
                        
                        if (!sizes.Contains(size))
                        {
                            sizes.Add(size);
                        }           
                    }                  
                }

                modelResponse.colors = colors;
                modelResponse.sizes = sizes;

                if (colorsAndSizes != null)
                {
                    if (colorsAndSizes.Colors != null && colorsAndSizes.Sizes != null)
                    {
                        if (colorsAndSizes.Colors.All(colors.Contains) && colorsAndSizes.Sizes.All(sizes.Contains))
                        {
                            modelResponses.Add(modelResponse);
                        }
                    }

                    if (colorsAndSizes.Colors != null && colorsAndSizes.Sizes == null)
                    {
                        if (colorsAndSizes.Colors.All(colors.Contains))
                        {
                            modelResponses.Add(modelResponse);
                        }
                    }

                    if (colorsAndSizes.Colors == null && colorsAndSizes.Sizes != null)
                    {
                        if (colorsAndSizes.Sizes.All(sizes.Contains))
                        {
                            modelResponses.Add(modelResponse);
                        }
                    }
                } 
                else
                {
                    modelResponses.Add(modelResponse);
                } 
            }

            Pager<ModelResponse> modelsPaginator = new Pager<ModelResponse>
                (
                    modelResponses,
                    perPage,
                    page
                );

            return Ok(modelsPaginator);
        }

        /**<summary> 
         * Function that returns a model with your Id, and OrganizationId
         * </summary>
         * <param name="id"> modelId </param>
         * <param name="idOrg"> organizationId </param>
         */
        [HttpGet("getModelById")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<Model>> GetModel([FromQuery][BindRequired] string id)
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

            var model = await _context.Models.FindAsync(id, idOrg);
            if (model == null)
            {
                throw new ModelArgumentException("Model not found.");
            }

            return Ok(model);
        }


        /**<summary> 
         * Function that returns all models by status
         * </summary>
         * <param name="status"> status </param>
         */
        [HttpGet("getModelsByStatus")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Model>>> GetModelsByStatus([FromQuery][BindRequired] int status)
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

            List<Model> models = new List<Model>();

            if (status == 0)
            {
                models = await _context.Models
                .Where(p => p.DeletedAt != null && p.OrganizationId == idOrg)
                .ToListAsync();
            } 
            else
            {
                models = await _context.Models
                .Where(p => p.DeletedAt == null && p.OrganizationId == idOrg)
                .ToListAsync();
            }

            if (models.Count == 0)
            {
                throw new ModelArgumentException("No deleted Models found.");
            }

            return Ok(models);
        }


        /**<summary> 
        * Add new model
        * </summary>
        * <param name="modelRequest">the model to be added </param>
        */
        [HttpPost]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Model>>> AddModel([Required][FromForm] ModelsRequest modelRequest)
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
                Model model = modelRequest.model;
                model.OrganizationId = idOrg;
                List<Color> colors = modelRequest.colors;
                List<Size> sizes = modelRequest.sizes;
                List<Component> components = modelRequest.components;

                ModelRequestValidator.Validate(model);

                if (colors == null)
                {
                    throw new ModelArgumentException("Colors list is null");
                }

                else if (sizes == null)
                {
                    throw new ModelArgumentException("Sizes list is null");
                }

                else if (components == null)
                {
                    throw new ModelArgumentException("Components list is null");
                }

                if (await _context.Models.FindAsync(model.Id, model.OrganizationId) != null)
                {
                    throw new ModelArgumentException("Model already exists!");
                }
                
                if (modelRequest.image != null)
                {
                    Images images = new Images(_webHostEnvironment);
                    string? urlImage = await images.saveImage(modelRequest.image);

                    if (urlImage == null)
                    {
                        throw new UserArgumentException("Invalid image");
                    }

                    modelRequest.model.ModelPhoto = urlImage;
                }

                _context.Models.Add(model);

                await AddColorsAndSizeToModel(model, colors, sizes);
                await AddComponentsToModel(model, components);

                await _context.SaveChangesAsync();

                return Ok(await _context.Models.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /** <summary>
         * Update an existant model
         * </summary>
         * <param name="modelRequest">the model to be updated</param> 
         */
        [HttpPut]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Model>>> UpdateModel([Required][FromForm] ModelsRequest modelRequest)
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
                Model model = modelRequest.model;
                model.OrganizationId = idOrg;
                List<Color> colors = modelRequest.colors;
                List<Size> sizes = modelRequest.sizes;
                List<Component> components = modelRequest.components;

                ModelRequestValidator.Validate(model);

                if (colors == null)
                {
                    throw new ModelArgumentException("Colors list is null");
                }

                else if (sizes == null)
                {
                    throw new ModelArgumentException("Sizes list is null");
                }

                else if (components == null)
                {
                    throw new ModelArgumentException("Components list is null");
                }

                var result = await _context.Models.FindAsync(model.Id, model.OrganizationId);
                if ( result == null || result.DeletedAt != null)
                {
                    throw new ModelArgumentException("Model not found!");
                }

                await RemoveAllColorsAndSizeFromModel(model.OrganizationId, model);
                await RemoveAllComponnetsFromModel(model.OrganizationId, model);

                await AddColorsAndSizeToModel(model, colors, sizes);
                await AddComponentsToModel(model, components);

                /*if (modelRequest.image != null)
                {
                    Images images = new Images(_webHostEnvironment);
                    string? urlImage = await images.saveImage(modelRequest.image);

                    if (urlImage == null)
                    {
                        throw new UserArgumentException("Invalid image");
                    }

                    result.ModelPhoto = urlImage;
                }*/

                await _context.SaveChangesAsync();

                return Ok(await _context.Models.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**<summary>
         * Remove all colors and sizes associated to respective model
         * </summary>
         * <param name="model">The model to remove colors and sizes list</param>
         * <param name="organizationId">The model organization</param>
         */
        private async Task RemoveAllColorsAndSizeFromModel(int organizationId, Model model)
        {

            var modelsColorsSizes = await _context.ModelsSizesColors
                .Where(p => p.ModelId == model.Id &&
                            p.OrganizationId == organizationId)
                .ToListAsync();

            _context.ModelsSizesColors.RemoveRange(modelsColorsSizes);
        }

        /**<summary>
         * Remove all components associated to respective model
         * </summary>
         * <param name="model">The model to remove components</param>
         * <param name="organizationId">The model organization</param>
         */
        private async Task RemoveAllComponnetsFromModel(int organizationId, Model model)
        {

            var modelsComponent = await _context.ModelsComponents
                .Where(p => p.ModelId == model.Id &&
                            p.OrganizationId == organizationId)
                .ToListAsync();

            _context.ModelsComponents.RemoveRange(modelsComponent);
        }

        /**<summary>
         * Associate colors and sizes to respective model
         * </summary>
         * <param name="colors">The color list to be added</param>
         * <param name="model">The model to associate color/size list</param>
         * <param name="sizes">The size list to be added</param>
         */
        private async Task AddColorsAndSizeToModel(Model model, List<Color> colors, List<Size> sizes)
        {
            foreach (var color in colors)
            {
                foreach (var size in sizes)
                {
                    if (await _context.ModelsSizesColors.FindAsync(
                            model.Id,
                            color.Id,
                            size.Id,
                            model.OrganizationId) != null)
                    {
                        throw new ModelArgumentException("Model already has this color and size associated!");
                    }

                    var colorFromDb = await _context.Colors.FindAsync(color.Id, color.OrganizationId);
                    var sizeFromDb = await _context.Sizes.FindAsync(size.Id, size.OrganizationId);

                    validateColor(color, colorFromDb);
                    validateSize(size, sizeFromDb);

                    //BUG: A comparação dá false mesmo quando os dois objetos são iguais
                    /*if (color != colorFromDb)
                    {
                        throw new ModelArgumentException("Any color from DB matches the specified data");
                    }
                    else if (!size.Equals(sizeFromDb))
                    {
                        throw new ModelArgumentException("Any size matches from DB the specified data");
                    }
                    else if (color.OrganizationId != model.OrganizationId)
                    {
                        throw new ModelArgumentException("The mentionated color don't exists on model organization");
                    }
                    else if (size.OrganizationId != model.OrganizationId)
                    {
                        throw new ModelArgumentException("The mentionated size don't exists on model organization");
                    }*/

                    ModelsSizesColor modelsSizesColor = new ModelsSizesColor();

                    modelsSizesColor.ModelId = model.Id;
                    modelsSizesColor.ColorId = color.Id;
                    modelsSizesColor.SizeId = size.Id;
                    modelsSizesColor.OrganizationId = model.OrganizationId;
                    modelsSizesColor.DeletedAt = model.DeletedAt;

                    _context.ModelsSizesColors.Add(modelsSizesColor);
                }
            }
        }

        /**
         * <summary>
         *  Check if color sent in request is the same of the database
         * </summary> 
         * <param name="color">The request color</param>
         * <param name="colorFromDb">The color from Database</param>
         */
        private bool validateColor(Color color, Color colorFromDb)
        {
            if (color.Color1 != colorFromDb.Color1 || color.Id != colorFromDb.Id || color.OrganizationId != colorFromDb.OrganizationId)
            {
                throw new ModelArgumentException("Any color from DB matches the specified data");
            }

            return true;
        }

        /**
         * <summary>
         *  Check if size sent in request is the same of the database
         * </summary> 
         * <param name="size">The request size</param>
         * <param name="sizeFromDb">The size from Database</param>
         */
        private bool validateSize(Size size, Size sizeFromDb)
        {
            
            if (size.Size1 != sizeFromDb.Size1 || size.Id != sizeFromDb.Id || size.OrganizationId != sizeFromDb.OrganizationId)
            {
                throw new ModelArgumentException("Any size from DB matches the specified data");
            }

            return true;
        }

        /**
         * <summary>
         *  Add components list to a model
         * </summary> 
         * <param name="model">The model where is components will be added</param>
         * <param name="component">The components list to be added</param>
         */
        private async Task AddComponentsToModel(Model model, List<Component> components)
        {
            foreach (var component in components)
            {
                ModelsComponent modelsComponent = new ModelsComponent();

                var componentFromDb = await _context.Components.FindAsync(component.Id, component.OrganizationId);

                validateComponent(component, componentFromDb);

                if (await _context.ModelsComponents.FindAsync(model.Id, component.Id, model.OrganizationId) != null)
                {
                    throw new ModelArgumentException("Model allready has this component associated!!");
                }

                /*
                if (model.OrganizationId != component.OrganizationId)
                {
                    throw new ModelArgumentException("This model and component do not belong to the same organization");
                }*/

                modelsComponent.ModelId = model.Id;
                modelsComponent.ComponentsId = component.Id;
                modelsComponent.OrganizationId = model.OrganizationId;
                modelsComponent.Amount = 0;

                _context.ModelsComponents.Add(modelsComponent);
            }
        }

        /**
         * <summary>
         *  Check if component sent in request is the same of the database
         * </summary> 
         * <param name="component">The request component</param>
         * <param name="component">The component from Database</param>
         */
        private bool validateComponent(Component component, Component componentFromDB)
        {
            if (component.Id != componentFromDB.Id || 
                component.ComponentsTypeId != componentFromDB.ComponentsTypeId ||
                component.ProviderId != componentFromDB.ProviderId ||
                component.DeletedAt != componentFromDB.DeletedAt || 
                component.OrganizationId != componentFromDB.OrganizationId)
            {
                throw new ModelArgumentException("Any component from DB matches the specified data");
            }

            return true;
        }

        /**
         * <summary>
         *  withdraw the model of the production
         * </summary> 
         * <param name="organizationId">The model organization</param>
         * <param name="id">The model id</param>
         */
        [HttpPut("outOfProduction")]
        public async Task<ActionResult<List<Model>>> OutOfProduction([FromQuery][BindRequired] string id)
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
                var result = await _context.Models.FindAsync(id, idOrg);

                if (result == null)
                {
                    throw new ModelArgumentException("Model not found!");
                }

                ModelRequestValidator.Validate(result);

                result.DeletedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(await _context.Models.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }

        /**
         * <summary>
         *  put back the model in production
         * </summary> 
         * <param name="organizationId">The model organization</param>
         * <param name="id">The model id</param>
         */
        [HttpPut("inProduction")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<List<Model>>> InProduction([FromQuery][BindRequired] string id)
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
                var result = await _context.Models.FindAsync(id, idOrg);

                if (result == null)
                {
                    throw new ModelArgumentException("Model not found!");
                }

                if (result.DeletedAt == null)
                {
                    throw new ModelArgumentException("This model is already in production!");
                }

                result.DeletedAt = null;

                await _context.SaveChangesAsync();

                return Ok(await _context.Models.ToListAsync());
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Some specified Id's are invalid! Please check again!");
            }
        }
    }
}
