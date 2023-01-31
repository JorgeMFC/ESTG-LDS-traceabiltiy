using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTrace.Utils;
using Route = MyTrace.Models.Route;

namespace MyTrace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
        private readonly MyTraceContext _context;
        private readonly JwtAuthenticationManager _jwtAuthenticationManager;
        public RoutesController(
            MyTraceContext context,
            JwtAuthenticationManager jwtAuthenticationManager
        )
        {
            _context = context;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpPost("IsPermittedRoute")]
        [AllowAnonymous]
        [SwaggerHeader(false)]
        public async Task<ActionResult<UsersTypeRoute>> IsPermittedRoute([FromBody] string routes)
        {
            byte userTypeId = 6;

            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet != null)
            {
                User? user = await _context.Users.FindAsync(wallet);
                if (user!=null && user.DeletedAt == null)
                {
                    userTypeId = user.UserTypeId;
                }
            }

            UsersTypeRoute? usersTypeRoute = await _context.UsersTypeRoutes.FindAsync(userTypeId, routes);

            if (usersTypeRoute == null)
            {
                usersTypeRoute = new UsersTypeRoute();
                usersTypeRoute.Route = routes;
                usersTypeRoute.UserTypeId = userTypeId;
                usersTypeRoute.Permissions = "UnAuthorized";

            }

            return Ok(usersTypeRoute);
        }

        [HttpPost("IsPermittedRoutes")]
        [AllowAnonymous]
        [SwaggerHeader(false)]
        public async Task<ActionResult<UsersTypeRoute?>> IsPermittedRoutes([FromBody] List<string> routes)
        {
            byte userTypeId = 6;

            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet != null)
            {
                User? user = await _context.Users.FindAsync(wallet);
                if (user != null && user.DeletedAt == null)
                {
                    userTypeId = user.UserTypeId;
                }
            }

            //List <UsersTypeRoute?> usersTypeRoute = await _context.UsersTypeRoutes.Where(p => p.UserTypeId = userTypeId).ToListAsync();

            List<UsersTypeRoute?> usersTypeRoute = await _context.UsersTypeRoutes
                .Where(p => p.UserTypeId == userTypeId && routes.Contains(p.Route)
                )
                .ToListAsync();

            return Ok(usersTypeRoute);
        }
    }
}
