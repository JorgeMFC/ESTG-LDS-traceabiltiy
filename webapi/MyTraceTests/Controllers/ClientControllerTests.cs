using Azure;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTrace.Controllers;
using MyTrace.Domain.Exceptions;
using MyTrace.Models;
using MyTrace.Utils;
using NBitcoin.Secp256k1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyTrace.Controllers.Tests
{
    [TestClass()]
    public class ClientControllerTests
    {
        /*private readonly MyTraceContext _context = new MyTraceContext();
        private ClientController clientController;
        private CLientAndClientAddress clientAndClientAddress = new CLientAndClientAddress();
        private ClientsAddress clientAddress = new ClientsAddress();
        private Client client = new Client();

        public ClientControllerTests()
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

            clientController = new ClientController(_context, jwtAuthenticationManager);

            client.Id = "C001";
            client.OrganizationId = 1;
            client.Email = "luisousa2002@gmail.com";
            client.Name = "Luis";

            clientAddress.Id = "CA001";
            clientAddress.ClientId = "C001";
            clientAddress.OrganizationId = 1;
            clientAddress.Address = "Rua da pedra alta";
            clientAddress.Zipcode = "1234-123";

            clientAndClientAddress.client = client;
            clientAndClientAddress.clientsAddressesList = new List<ClientsAddress>();
        }*/

        /** <summary>
         * Test add client null
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddClientTestNull()
        {
            Regex regex = new Regex(@"^(client is null!)$");
            client = null;

            try
            {
                await clientController.AddClient(client);
            }
            catch (ClientArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add client with an empty name
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddClientTestEmptyName()
        {
            Regex regex = new Regex(@"^(Name is Null or Empty)$");
            client.Name = "";

            try
            {
                await clientController.AddClient(client);
            }
            catch (ClientArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add client with a null name
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddClientTestNullName()
        {
            Regex regex = new Regex(@"^(Name is Null or Empty)$");
            client.Name = null;

            try
            {
                await clientController.AddClient(client);
            }
            catch (ClientArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add client with an empty email
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddClientTestEmptyEmail()
        {
            Regex regex = new Regex(@"^(Client email is Null or Empty)$");
            client.Email = "";

            try
            {
                await clientController.AddClient(client);
            }
            catch (ClientArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add client with a null email
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddClientTestNullEmail()
        {
            Regex regex = new Regex(@"^(Client email is Null or Empty)$");
            client.Email = null;

            try
            {
                await clientController.AddClient(client);
            }
            catch (ClientArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add client with an invalid email, that is email does not contain @ annotation 
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddClientTestInvalidEmail()
        {
            Regex regex = new Regex(@"^(Client email format is not valid!)$");
            client.Email = "josegmail.com";

            try
            {
                await clientController.AddClient(client);
            }
            catch (ClientArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update client null
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateClientTestNull()
        {
            Regex regex = new Regex(@"^(client is null!)$");
            client = null;

            try
            {
                await clientController.UpdateClient(client);
            }
            catch (ClientArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update client with an empty name
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateClientTestEmptyName()
        {
            Regex regex = new Regex(@"^(Name is Null or Empty)$");
            client.Name = "";

            try
            {
                await clientController.UpdateClient(client);
            }
            catch (ClientArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update client with a null name
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateClientTestNullName()
        {
            Regex regex = new Regex(@"^(Name is Null or Empty)$");
            client.Name = null;

            try
            {
                await clientController.UpdateClient(client);
            }
            catch (ClientArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update client with an empty email
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateClientTestEmptyEmail()
        {
            Regex regex = new Regex(@"^(Client email is Null or Empty)$");
            client.Email = "";

            try
            {
                await clientController.UpdateClient(client);
            }
            catch (ClientArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update client with a null email
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateClientTestNullEmail()
        {
            Regex regex = new Regex(@"^(Client email is Null or Empty)$");
            client.Email = null;

            try
            {
                await clientController.UpdateClient(client);
            }
            catch (ClientArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update client with an invalid email, that is email does not contain @ annotation 
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateClientTestInvalidEmail()
        {
            Regex regex = new Regex(@"^(Client email format is not valid!)$");
            client.Email = "josegmail.com";

            try
            {
                await clientController.UpdateClient(client);
            }
            catch (ClientArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add clientAddress null
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddClientAddressTestNull()
        {
            Regex regex = new Regex(@"^(clientAddress is null!)$");
            clientAddress = null;

            try
            {
                await clientController.AddClientAddress(clientAddress);
            }
            catch (ClientArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add clientAddress with address empty
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddClientAddressTestEmptyAddress()
        {
            Regex regex = new Regex(@"^(Address is Null or Empty)$");
            clientAddress.Address = "";

            try
            {
                await clientController.AddClientAddress(clientAddress);
            }
            catch (ClientArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add clientAddress with address null
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddClientAddressTestNullAddress()
        {
            Regex regex = new Regex(@"^(Address is Null or Empty)$");
            clientAddress.Address = null;

            try
            {
                await clientController.AddClientAddress(clientAddress);
            }
            catch (ClientArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add clientAddress with zipcode empty
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddClientAddressTestEmptyZipcode()
        {
            Regex regex = new Regex(@"^(Zipcode is Null or Empty)$");
            clientAddress.Zipcode = "";

            try
            {
                await clientController.AddClientAddress(clientAddress);
            }
            catch (ClientArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add clientAddress with zipcode null
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddClientAddressTestNullZipcode()
        {
            Regex regex = new Regex(@"^(Zipcode is Null or Empty)$");
            clientAddress.Zipcode = null;

            try
            {
                await clientController.AddClientAddress(clientAddress);
            }
            catch (ClientArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update clientAddress with an invalid zipcode
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddClientTestInvalidZipcode()
        {
            Regex regex = new Regex(@"^(ClientAddress zipcode format is not valid!)$");
            clientAddress.Zipcode = "12333";

            try
            {
                await clientController.AddClientAddress(clientAddress);
            }
            catch (ClientArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/
    }
}