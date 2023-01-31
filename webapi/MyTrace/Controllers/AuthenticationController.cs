using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTrace.Blockchain;
using MyTrace.Utils;
using System.ComponentModel.DataAnnotations;

namespace MyTrace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly string? _baseSignatureMessage;
        private readonly MyTraceContext _context;
        private readonly JwtAuthenticationManager _jwtAuthenticationManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AuthenticationController(
            MyTraceContext context, 
            IConfiguration configuration,
            JwtAuthenticationManager jwtAuthenticationManager,
            IWebHostEnvironment webHostEnvironment
        )
        {
            _context = context;
            _baseSignatureMessage = configuration.GetSection("AppSettings:BaseSignatureMessage").Value;
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _webHostEnvironment = webHostEnvironment;
        }

        private string _GetSignatureMessage(User? user)
        {
            if (user == null)
            {
                return $"{_baseSignatureMessage}0";
            }

            return $"{_baseSignatureMessage}{user.Nonce}";
        }

        /**<summary>
         * Return a message to be signed by the wallet
         * </summary> 
         * <param name="wallet">the user wallet</param>
         */
        [HttpPost("getSignatureMessage")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> GetSignatureMessage([Required][FromBody] string wallet)
        {
            if (!Wallet.IsValidEthereumAddress(wallet))
            {
                throw new AuthenticationArgumentException("Invalid Wallet Address");
            }
            User? user = await _context.Users.FindAsync(wallet);
            return Ok(_GetSignatureMessage(user));
        }

        /**<summary>
         * Return a token to be used in the header
         * </summary> 
         * <param name="loginRequest">the wallet and the signature confirmation of the wallet</param>
         */
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login([Required][FromBody] LoginRequest loginRequest)
        {
            if (!Wallet.IsValidEthereumAddress(loginRequest.wallet))
            {
                throw new AuthenticationArgumentException("Invalid Wallet Address");
            }
            try
            {
                User? user = await _context.Users.FindAsync(loginRequest.wallet);
                string signatureMessage = _GetSignatureMessage(user);

                if (!Wallet.VerifySignature(loginRequest.wallet, signatureMessage, loginRequest.signature))
                {
                    throw new AuthenticationArgumentException("Invalid signature");
                }

                if (user != null)
                {
                    //edit nonce
                    user.Nonce ++;
                    _context.Users.Update(user);
                } else {
                    //regist
                    user = new User
                    {
                        WalletAddress = loginRequest.wallet,
                        Nonce = 1,
                        UserTypeId = 5
                    };

                    _context.Users.Add(user);
                }
                await _context.SaveChangesAsync();

                return Ok(_jwtAuthenticationManager.CreateToken(user));
            } catch (DbUpdateException) {
                throw new AuthenticationArgumentException("Error");
            }
        }

        [HttpPut("regist")]
        [Authorize]
        [SwaggerHeader(true)]
        public async Task<ActionResult<string>> Regist([Required][FromForm] RegistRequest registRequest)
        {
            string? wallet = _jwtAuthenticationManager.GetUserWallet(
                HttpContext.Request.Headers["Authorization"]
            );

            if (wallet == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            RegistRequestValidator.Validate(registRequest);

            User? user = await _context.Users.FindAsync(wallet);

            if (user == null)
            {
                throw new AuthenticationArgumentException("Invalid token");
            }

            if (user.UserTypeId != 5 || user.DeletedAt != null)
            {
                throw new AuthenticationArgumentException("User already registered");
            }

            if (registRequest.avatar != null)
            {
                Images images = new Images(_webHostEnvironment);
                string? urlImage = await images.saveImage(registRequest.avatar);

                if (urlImage == null)
                {
                    throw new AuthenticationArgumentException("Invalid image");
                }

                user.Avatar = urlImage;
            }

            try
            {
                user.Email = registRequest.Email;
                user.Name = registRequest.Name;
                user.UserTypeId = 4;

                if (registRequest.BirthDate != null)
                {
                    user.BirthDate = registRequest.BirthDate;
                }

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                throw new AuthenticationArgumentException("Error saving data");
            }

            return Ok("Registration successfully");
        }
    }
}
