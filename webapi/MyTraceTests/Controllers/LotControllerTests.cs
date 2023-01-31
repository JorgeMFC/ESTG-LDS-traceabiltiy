using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
    public class LotControllerTests
    {
        /*private readonly MyTraceContext _context = new MyTraceContext();
        LotController lotController;
        Regex regex;
        private Lot lot = new Lot();

        public LotControllerTests()
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

            IOptions<CryptocurrencyBrokerageEnv> cryptocurrencyBrokerageEnv = Options.Create<CryptocurrencyBrokerageEnv>(new CryptocurrencyBrokerageEnv());
            IOptions<PinataEnv> pinataEnv = Options.Create<PinataEnv>(new PinataEnv());
            lotController = new LotController(_context, cryptocurrencyBrokerageEnv, pinataEnv, null, jwtAuthenticationManager);

            lot.Id = "LT001";
            lot.OrganizationId = 1;
            lot.ModelId = "MO001";
            lot.ModelColorId = "CO001";
            lot.ModelSizeId = "SI001";
            lot.ClientId = "CL001";
            lot.ClientAddressId = "ADS001";
            lot.OrganizationAddressId = "OADS001";
            lot.DeliveryDate = DateTime.Now;
            lot.LotSize = 10;
            lot.Hash = "abvG4HAd8";
            lot.StagesModelId = "SM001";
            lot.CanceledAt = null;
        }*/

        /** <summary>
        * Test add a lot as null
        * </summary>
        */
        /*[TestMethod()]
        public async Task AddNullLotTest()
        {
            regex = new Regex(@"^(lot is null!)$");
            lot = null;

            try
            {
                await lotController.AddLot(lot);
            }
            catch (LotArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
        * Test add a lot with an id as null
        * </summary>
        */
        /*[TestMethod()]
        public async Task AddLotTestNullId()
        {
            regex = new Regex(@"^(Id is null or empty)$");
            lot.Id = null;

            try
            {
                await lotController.AddLot(lot);
            }
            catch (LotArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
        * Test add a lot with an empty id
        * </summary>
        */
        /*[TestMethod()]
        public async Task AddLotTestEmptyId()
        {
            regex = new Regex(@"^(Id is null or empty)$");
            lot.Id = "";

            try
            {
                await lotController.AddLot(lot);
            }
            catch (LotArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
        * Test add a lot with a hash as null
        * </summary>
        */
        /*[TestMethod()]
        public async Task AddLotTestNullHash()
        {
            regex = new Regex(@"^(Hash is Null or Empty)$");
            lot.Hash = null;

            try
            {
                await lotController.AddLot(lot);
            }
            catch (LotArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
        * Test add a lot with an empty hash
        * </summary>
        */
        /*[TestMethod()]
        public async Task AddLotTestEmptyHash()
        {
            regex = new Regex(@"^(Hash is Null or Empty)$");
            lot.Hash = null;

            try
            {
                await lotController.AddLot(lot);
            }
            catch (LotArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
        * Test add a lot with an invalid canceled date
        * </summary>
        */
        /*[TestMethod()]
        public async Task AddLotTestInvalidCanceledDate()
        {
            regex = new Regex(@"^(This lot can't be created or updated because has been canceled!)$");
            lot.CanceledAt = DateTime.Now;

            try
            {
                await lotController.AddLot(lot);
            }
            catch (LotArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/


        /** <summary>
       * Test update a lot with an id as null
       * </summary>
       */
        /*/*[TestMethod()]
        public async Task UpdateLotTestNullId()
        {
            regex = new Regex(@"^(Id is null or empty)$");
            lot.Id = null;

            try
            {
                await lotController.UpdateLot(lot);
            }
            catch (LotArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
        * Test update a lot with an empty id
        * </summary>
        */
        /*/*[TestMethod()]
        public async Task UpdateLotTestEmptyId()
        {
            regex = new Regex(@"^(Id is null or empty)$");
            lot.Id = "";

            try
            {
                await lotController.UpdateLot(lot);
            }
            catch (LotArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
        * Test update a lot with a hash as null
        * </summary>
        */
        /*/*[TestMethod()]
        public async Task UpdateLotTestNullHash()
        {
            regex = new Regex(@"^(Hash is Null or Empty)$");
            lot.Hash = null;

            try
            {
                await lotController.UpdateLot(lot);
            }
            catch (LotArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
        * Test update a lot with an empty hash
        * </summary>
        */
        /*/*[TestMethod()]
        public async Task UpdateLotTestEmptyHash()
        {
            regex = new Regex(@"^(Hash is Null or Empty)$");
            lot.Hash = null;

            try
            {
                await lotController.UpdateLot(lot);
            }
            catch (LotArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
        * Test update a lot with an invalid canceled date
        * </summary>
        */
        /*/*[TestMethod()]
        public async Task UpdateLotTestInvalidCanceledDate()
        {
            regex = new Regex(@"^(This lot can't be created or updated because has been canceled!)$");
            lot.CanceledAt = DateTime.Now;

            try
            {
                await lotController.UpdateLot(lot);
            }
            catch (LotArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/
    }
}