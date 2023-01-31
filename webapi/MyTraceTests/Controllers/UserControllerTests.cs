using Azure;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class UserControllerTests
    {
        /*private readonly MyTraceContext _context = new MyTraceContext();
        UserController userController;
        Regex regex;
        private User user = new User();

        public UserControllerTests()
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

            userController = new UserController(_context, jwtAuthenticationManager);

            user.WalletAddress = "1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa";
            user.Nonce = 0;
            user.Name = "jose";
            user.Email = "teste@gmail.com";
            user.BirthDate = null;
            user.CreatedAt = DateTime.Now;
            user.OrganizationId = 1;
            user.UserTypeId = 4;
        }*/

        /** <summary>
         * Test update user with an empty wallet
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateUserTestEmptyWallet()
        {
            regex = new Regex(@"^(WalletAddress is Null or Empty)$");
            user.WalletAddress = "";

            try
            {
                await userController.UpdateUser(user);
            }
            catch (UserArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update user with an null wallet
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateUserTestNullWallet()
        {
            regex = new Regex(@"^(WalletAddress is Null or Empty)$");
            user.WalletAddress = null;

            try
            {
                await userController.UpdateUser(user);
            }
            catch (UserArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update user with an empty name
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateUserTestEmptyName()
        {
            regex = new Regex(@"^(Name is Null or Empty)$");
            user.Name = "";

            try
            {
                await userController.UpdateUser(user);
            }
            catch (UserArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update user with a null name
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateUserTestNullName()
        {
            regex = new Regex(@"^(Name is Null or Empty)$");
            user.Name = null;

            try
            {
                await userController.UpdateUser(user);
            }
            catch (UserArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update user with an empty email
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateUserTestEmptyEmail()
        {
            regex = new Regex(@"^(User email is Null or Empty)$");
            user.Email = "";

            try
            {
                await userController.UpdateUser(user);
            }
            catch (UserArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update user with a null email
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateUserTestNullEmail()
        {
            regex = new Regex(@"^(User email is Null or Empty)$");
            user.Email = null;

            try
            {
                await userController.UpdateUser(user);
            }
            catch (UserArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update user with an invalid email, that is email does not contain @ annotation 
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateUserTestInvalidEmail()
        {
            regex = new Regex(@"^(User email format is not valid!)$");
            user.Email = "josegmail.com";

            try
            {
                await userController.UpdateUser(user);
            }
            catch (UserArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update user with a deletedAt Field not as null, that is update an user that's already desactivated before being added
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateUserWithInvalidDeletedFieldTest()
        {
            regex = new Regex(@"^(This is user can't be created or updated because has been desactivated!)$");
            user.DeletedAt = DateTime.Now;

            try
            {
                await userController.UpdateUser(user);
            }
            catch (UserArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update user with a invalid created account date, that is the update account date is greater than current date
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateUserdTestInvalidCreatedDate()
        {
            regex = new Regex(@"^(User account createdDate cannot be greater than current date)$");
            user.CreatedAt = DateTime.Now.AddDays(1);

            try
            {
                await userController.UpdateUser(user);
            }
            catch (UserArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/
    }
}