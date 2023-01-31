using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTrace.Controllers;
using MyTrace.Domain.Exceptions;
using MyTrace.Models;
using MyTrace.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyTrace.Controllers.Tests
{
    [TestClass()]
    public class OrganizationControllerTests
    {
        /*private readonly MyTraceContext _context = new MyTraceContext();
        OrganizationController organizationController;
        Regex regex;
        private Organization organization = new Organization();

        public OrganizationControllerTests()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"AppSettings:BaseSignatureMessage", "Sign this message to confirm you own this wallet address. This action will not cost any gas fees.\n\nNonce: "},
                {"AppSettings:JWT:ScretKey", "Sign this message to confirm you own this wallet address. This action will not cost any gas fees.\n\nNonce: "}
            };

            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();

            JwtAuthenticationManager jwtAuthenticationManager = new JwtAuthenticationManager(configuration);

            organizationController = new OrganizationController(_context, jwtAuthenticationManager);

            organization.Id = 1;
            organization.Name = "Organizacao 1";
            organization.Logo = "logo.png";
            organization.Email = "organization1@gmail.com";
            organization.WalletAddress = "1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa";
        }*/

        /** <summary>
         * Test add organization with an empty name
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddOrganizationTestEmptyName()
        {
            regex = new Regex(@"^(Name is Null or Empty)$");
            organization.Name = "";

            try
            {
                await organizationController.AddOrganization(organization);
            }
            catch (OrganizationArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add organization with a null name
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddOrganizationTestNullName()
        {
            regex = new Regex(@"^(Name is Null or Empty)$");
            organization.Name = null;

            try
            {
                await organizationController.AddOrganization(organization);
            }
            catch (OrganizationArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add organization with an empty email
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddOrganizationTestEmptyEmail()
        {
            regex = new Regex(@"^(Email is Null or Empty)$");
            organization.Email = "";

            try
            {
                await organizationController.AddOrganization(organization);
            }
            catch (OrganizationArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add organization with a null email
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddOrganizationTestNullEmail()
        {
            regex = new Regex(@"^(Email is Null or Empty)$");
            organization.Email = null;

            try
            {
                await organizationController.AddOrganization(organization);
            }
            catch (OrganizationArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add organization with an invalid email
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddOrganizationTestInvalidEmail()
        {
            regex = new Regex(@"^(Organization email format is not valid!)$");
            organization.Email = "organization1gmail.com";

            try
            {
                await organizationController.AddOrganization(organization);
            }
            catch (OrganizationArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add organization with an empty wallet
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddOrganizationTestEmptyWallet()
        {
            regex = new Regex(@"^(WalletAddress is Null or Empty)$");
            organization.WalletAddress = "";

            try
            {
                await organizationController.AddOrganization(organization);
            }
            catch (OrganizationArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add organization with a null wallet
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddOrganizationTestNullWallet()
        {
            regex = new Regex(@"^(WalletAddress is Null or Empty)$");
            organization.WalletAddress = null;

            try
            {
                await organizationController.AddOrganization(organization);
            }
            catch (OrganizationArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/


        //-------------------------------------

        /** <summary>
         * Test update organization with an empty name
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateOrganizationTestEmptyName()
        {
            regex = new Regex(@"^(Name is Null or Empty)$");
            organization.Name = "";

            try
            {
                await organizationController.UpdateOrganization(organization);
            }
            catch (OrganizationArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update organization with a null name
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateOrganizationTestNullName()
        {
            regex = new Regex(@"^(Name is Null or Empty)$");
            organization.Name = null;

            try
            {
                await organizationController.UpdateOrganization(organization);
            }
            catch (OrganizationArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update organization with an empty email
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateOrganizationTestEmptyEmail()
        {
            regex = new Regex(@"^(Email is Null or Empty)$");
            organization.Email = "";

            try
            {
                await organizationController.UpdateOrganization(organization);
            }
            catch (OrganizationArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update organization with a null email
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateOrganizationTestNullEmail()
        {
            regex = new Regex(@"^(Email is Null or Empty)$");
            organization.Email = null;

            try
            {
                await organizationController.UpdateOrganization(organization);
            }
            catch (OrganizationArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update organization with an inavalid email
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateOrganizationTestInvalidEmail()
        {
            regex = new Regex(@"^(Organization email format is not valid!)$");
            organization.Email = "organization1gmail.com";

            try
            {
                await organizationController.UpdateOrganization(organization);
            }
            catch (OrganizationArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update organization with an empty wallet
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateOrganizationTestEmptyWallet()
        {
            regex = new Regex(@"^(WalletAddress is Null or Empty)$");
            organization.WalletAddress = "";

            try
            {
                await organizationController.UpdateOrganization(organization);
            }
            catch (OrganizationArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update organization with a null wallet
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateOrganizationTestNullWallet()
        {
            regex = new Regex(@"^(WalletAddress is Null or Empty)$");
            organization.WalletAddress = null;

            try
            {
                await organizationController.UpdateOrganization(organization);
            }
            catch (OrganizationArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/
    }
}