using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTrace.Controllers;
using MyTrace.Domain.Exceptions;
using MyTrace.Models;
using MyTrace.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyTrace.Controllers.Tests
{
    [TestClass()]
    public class StagesModelControllerTests
    {
        /*private readonly MyTraceContext _context = new MyTraceContext();
        StagesModelController stagesModelController;
        private StagesModel stagesModel = new StagesModel();

        public StagesModelControllerTests()
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

            stagesModelController = new StagesModelController(_context, jwtAuthenticationManager);
            stagesModel.Id = "SM001";
            stagesModel.OrganizationId = 1;
            stagesModel.StagesModelName = "Stage Model 1";
        }*/

        /**<summary>
         * Test add null stagesModel 
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddStagesModelTestNull()
        {
            Regex regex = new Regex(@"^(stagesModel is null!)$");
            stagesModel = null;

            try
            {
                await stagesModelController.AddStagesModel(stagesModel);
            }
            catch (StagesModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test add stagesModel with empty Id
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddStagesModelTestEmptyId()
        {
            Regex regex = new Regex(@"^(StagesModelId is Null or Empty)$");
            stagesModel.Id = "";

            try
            {
                await stagesModelController.AddStagesModel(stagesModel);
            }
            catch (StagesModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test add stagesModel with null Id
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddStagesModelTestNullId()
        {
            Regex regex = new Regex(@"^(StagesModelId is Null or Empty)$");
            stagesModel.Id = null;

            try
            {
                await stagesModelController.AddStagesModel(stagesModel);
            }
            catch (StagesModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/


        /**<summary>
         * Test add stagesModel with empty stagesModelName
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddStagesModelTestStagesModelNameIsEmpty()
        {
            Regex regex = new Regex(@"^(StagesModelName is Null or Empty)$");
            stagesModel.StagesModelName = "";

            try
            {
                await stagesModelController.AddStagesModel(stagesModel);
            }
            catch (StagesModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test add stagesModel with null stagesModelName
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddStagesModelTestStagesModelNameIsNull()
        {
            Regex regex = new Regex(@"^(StagesModelName is Null or Empty)$");
            stagesModel.StagesModelName = null;

            try
            {
                await stagesModelController.AddStagesModel(stagesModel);
            }
            catch (StagesModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test update null stagesModel 
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateStagesModelTestNull()
        {
            Regex regex = new Regex(@"^(stagesModel is null!)$");
            stagesModel = null;

            try
            {
                await stagesModelController.UpdateStagesModel(stagesModel);
            }
            catch (StagesModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test update stagesModel with empty Id
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateStagesModelTestEmptyId()
        {
            Regex regex = new Regex(@"^(StagesModelId is Null or Empty)$");
            stagesModel.Id = "";

            try
            {
                await stagesModelController.UpdateStagesModel(stagesModel);
            }
            catch (StagesModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test update stagesModel with null Id
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateStagesModelTestNullId()
        {
            Regex regex = new Regex(@"^(StagesModelId is Null or Empty)$");
            stagesModel.Id = null;

            try
            {
                await stagesModelController.UpdateStagesModel(stagesModel);
            }
            catch (StagesModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test update stagesModel with empty stagesModelName
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateStagesModelTestStagesModelNameIsEmpty()
        {
            Regex regex = new Regex(@"^(StagesModelName is Null or Empty)$");
            stagesModel.StagesModelName = "";

            try
            {
                await stagesModelController.UpdateStagesModel(stagesModel);
            }
            catch (StagesModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test update stagesModel with null stagesModelName
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateStagesModelTestStagesModelNameIsNull()
        {
            Regex regex = new Regex(@"^(StagesModelName is Null or Empty)$");
            stagesModel.StagesModelName = null;

            try
            {
                await stagesModelController.UpdateStagesModel(stagesModel);
            }
            catch (StagesModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/
    }
}