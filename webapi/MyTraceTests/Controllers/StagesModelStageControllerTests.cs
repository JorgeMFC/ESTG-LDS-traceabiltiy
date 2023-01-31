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
    public class StagesModelStageControllerTests
    {
        /*private readonly MyTraceContext _context = new MyTraceContext();
        StagesModelStageController stagesModelStageController;
        private StagesModelStage stagesModelStage = new StagesModelStage();

        public StagesModelStageControllerTests()
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

            stagesModelStageController = new StagesModelStageController(_context, jwtAuthenticationManager);
            stagesModelStage.StagesModelId = "SM001";
            stagesModelStage.StagesId = "S001";
            stagesModelStage.OrganizationId = 1;
        }*/

        /**<summary>
         * Test add null stagesModelStage 
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddStagesModelStageTestNull()
        {
            Regex regex = new Regex(@"^(stagesModelStage is null!)$");
            stagesModelStage = null;

            try
            {
                await stagesModelStageController.AddStagesModelStage(stagesModelStage);
            }
            catch (StagesModelStageArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/
    }
}