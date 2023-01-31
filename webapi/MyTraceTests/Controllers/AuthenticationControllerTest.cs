using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTrace.Controllers;
using MyTrace.Domain.Exceptions;
using MyTrace.Models;
using MyTrace.Utils;
using System.Text.RegularExpressions;

namespace MyTraceTests.Controllers
{
    [TestClass()]
    public class AuthenticationControllerTest
    {
        private readonly MyTraceContext _context = new MyTraceContext();
        private readonly AuthenticationController authenticationController;

        public AuthenticationControllerTest()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"AppSettings:BaseSignatureMessage", "Sign this message to confirm you own this wallet address. This action will not cost any gas fees.\n\nNonce: "},
                {"AppSettings:JWT:ScretKey", "Sign this message to confirm you own this wallet address. This action will not cost any gas fees.\n\nNonce: "}
            };

            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();

            IWebHostEnvironment webHostEnvironment = null; 

            JwtAuthenticationManager jwtAuthenticationManager = new JwtAuthenticationManager(configuration);
            authenticationController = new AuthenticationController(_context, configuration, jwtAuthenticationManager, webHostEnvironment);
        }

        [TestMethod()]
        public async Task GetSignatureMessageTestNull()
        {
            Regex regex = new Regex(@"^(Invalid Wallet Address)$");

            string wallet = null;

            try
            {
                await authenticationController.GetSignatureMessage(wallet);
            }
            catch (AuthenticationArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }

        [TestMethod()]
        public async Task GetSignatureMessageTestEmpty()
        {
            Regex regex = new Regex(@"^(Invalid Wallet Address)$");
            string wallet = "";

            try
            {
                await authenticationController.GetSignatureMessage(wallet);
            }
            catch (AuthenticationArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }

        [TestMethod()]
        public async Task LoginTestNull()
        {
            Regex regex = new Regex(@"^(Invalid Wallet Address)$");
            LoginRequest loginRequest = new LoginRequest();
            loginRequest.wallet = null;
            loginRequest.signature = null;

            try
            {
                await authenticationController.Login(loginRequest);
            }
            catch (AuthenticationArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }

        [TestMethod()]
        public async Task LoginTestEmpty()
        {
            Regex regex = new Regex(@"^(Invalid Wallet Address)$");
            LoginRequest loginRequest = new LoginRequest();
            loginRequest.wallet = "";
            loginRequest.signature = "";

            try
            {
                await authenticationController.Login(loginRequest);
            }
            catch (AuthenticationArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }
    }
}
