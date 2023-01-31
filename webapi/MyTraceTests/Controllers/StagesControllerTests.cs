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
    public class StagesControllerTests
    {
        /*private readonly MyTraceContext _context = new MyTraceContext();
        StagesController stagesController;
        Regex regex;
        private Stage stage = new Stage();

        public StagesControllerTests()
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

            stagesController = new StagesController(_context, jwtAuthenticationManager);

            stage.Id = "ST001";
            stage.OrganizationId = 1;
            stage.StageName = "Stage 1";
            stage.StagesTypeId = "Type 1";
            stage.StageDescription = "Stage Description";
        }*/

        /** <summary>
         * Test add stage with an empty Id
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddStagesTestEmptyId()
        {
            regex = new Regex(@"^(Id is Null or Empty)$");
            stage.Id = "";

            try
            {
                await stagesController.AddStage(stage);
            }
            catch (StageArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add stage with a null Id
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddStagesTestNullId()
        {
            regex = new Regex(@"^(Id is Null or Empty)$");
            stage.Id = null;

            try
            {
                await stagesController.AddStage(stage);
            }
            catch (StageArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add stage with an empty name
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddStagesTestEmptyName()
        {
            regex = new Regex(@"^(StageName is Null or Empty)$");
            stage.StageName = "";

            try
            {
                await stagesController.AddStage(stage);
            }
            catch (StageArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add stage with a null name
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddStagesTestNullName()
        {
            regex = new Regex(@"^(StageName is Null or Empty)$");
            stage.StageName = null;

            try
            {
                await stagesController.AddStage(stage);
            }
            catch (StageArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add stage with an empty stageDescription
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddStagesTestEmptyStageDescription()
        {
            regex = new Regex(@"^(StageDescription is Null or Empty)$");
            stage.StageDescription = "";

            try
            {
                await stagesController.AddStage(stage);
            }
            catch (StageArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add stage with a null stageDescription
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddStagesTestNullStageDescription()
        {
            regex = new Regex(@"^(StageDescription is Null or Empty)$");
            stage.StageDescription = null;

            try
            {
                await stagesController.AddStage(stage);
            }
            catch (StageArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add null stage
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddStagesTestNullStage()
        {
            regex = new Regex(@"^(stage is null!)$");
            stage = null;

            try
            {
                await stagesController.AddStage(stage);
            }
            catch (StageArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update stage with an empty Id
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateStagesTestEmptyId()
        {
            regex = new Regex(@"^(Id is Null or Empty)$");
            stage.Id = "";

            try
            {
                await stagesController.UpdateStage(stage);
            }
            catch (StageArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update stage with a null Id
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateStagesTestNullId()
        {
            regex = new Regex(@"^(Id is Null or Empty)$");
            stage.Id = null;

            try
            {
                await stagesController.UpdateStage(stage);
            }
            catch (StageArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update stage with an empty name
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateStagesTestEmptyName()
        {
            regex = new Regex(@"^(StageName is Null or Empty)$");
            stage.StageName = "";

            try
            {
                await stagesController.UpdateStage(stage);
            }
            catch (StageArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update stage with a null name
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateStagesTestNullName()
        {
            regex = new Regex(@"^(StageName is Null or Empty)$");
            stage.StageName = null;

            try
            {
                await stagesController.UpdateStage(stage);
            }
            catch (StageArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update stage with an empty stageDescription
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateStagesTestEmptyStageDescription()
        {
            regex = new Regex(@"^(StageDescription is Null or Empty)$");
            stage.StageDescription = "";

            try
            {
                await stagesController.UpdateStage(stage);
            }
            catch (StageArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update stage with a null stageDescription
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateStagesTestNullStageDescription()
        {
            regex = new Regex(@"^(StageDescription is Null or Empty)$");
            stage.StageDescription = null;

            try
            {
                await stagesController.UpdateStage(stage);
            }
            catch (StageArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update null stage
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateStagesTestNullStage()
        {
            regex = new Regex(@"^(stage is null!)$");
            stage = null;

            try
            {
                await stagesController.UpdateStage(stage);
            }
            catch (StageArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/
    }
}