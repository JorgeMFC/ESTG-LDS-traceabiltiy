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
    public class ModelControllerTests
    {
        /*private readonly MyTraceContext _context = new MyTraceContext();
        ModelController modelController;
        private Model model = new Model();
        private ModelsRequest modelRequest = new ModelsRequest();

        public ModelControllerTests()
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

            modelController = new ModelController(_context, jwtAuthenticationManager);
            model.Id = "MO0001";
            model.OrganizationId = 1;
            model.Name = "Modelo 1";
            model.DeletedAt = null;
            model.StagesModelId = "SM0001";

            modelRequest.model = model;
            modelRequest.colors = new List<Color>();
            modelRequest.sizes = new List<Size>();
            modelRequest.components = new List<Component>();
        }*/

        /*[TestMethod()]
        public async Task AddNullModelTest()
        {
            Regex regex = new Regex(@"^(model is null!)$");
            modelRequest.model = null;

            try
            {
                await modelController.AddModel(modelRequest);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }
        }*/

        /*[TestMethod()]
        public async Task AddModelNullColorsListTest()
        {
            Regex regex = new Regex(@"^(Colors list is null)$");
            modelRequest.colors = null;

            try
            {
                await modelController.AddModel(modelRequest);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }
        }*/

        /*[TestMethod()]
        public async Task AddModelNullSizesListTest()
        {
            Regex regex = new Regex(@"^(Sizes list is null)$");
            modelRequest.sizes = null;

            try
            {
                await modelController.AddModel(modelRequest);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }
        }*/

        /*[TestMethod()]
        public async Task AddModelNullComponentsListTest()
        {
            Regex regex = new Regex(@"^(Components list is null)$");
            modelRequest.components = null;

            try
            {
                await modelController.AddModel(modelRequest);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }
        }*/

        /*[TestMethod()]
        public async Task AddModelTestEmptyId()
        {
            Regex regex = new Regex(@"^(ModelId is null or empty)$");
            modelRequest.model.Id = "";

            try
            {
                await modelController.AddModel(modelRequest);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /*[TestMethod()]
        public async Task AddModelTestNullId()
        {
            Regex regex = new Regex(@"^(ModelId is null or empty)$");
            modelRequest.model.Id = null;

            try
            {
                await modelController.AddModel(modelRequest);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /*[TestMethod()]
        public async Task AddModelTestEmptyName()
        {
            Regex regex = new Regex(@"^(Name is Null or Empty)$");
            modelRequest.model.Name = "";

            try
            {
                await modelController.AddModel(modelRequest);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /*[TestMethod()]
        public async Task AddModelTestNullName()
        {
            Regex regex = new Regex(@"^(Name is Null or Empty)$");
            modelRequest.model.Name = null;

            try
            {
                await modelController.AddModel(modelRequest);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /*[TestMethod()]
        public async Task AddModelTestDeletedAtNotNull()
        {
            Regex regex = new Regex(@"^(This model can't be created or updated because has been desactivated!)$");
            modelRequest.model.DeletedAt = DateTime.Now;

            try
            {
                await modelController.AddModel(modelRequest);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/


        /*[TestMethod()]
        public async Task UpdateNullModelTest()
        {
            Regex regex = new Regex(@"^(model is null!)$");
            modelRequest.model = null;

            try
            {
                await modelController.UpdateModel(modelRequest);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }
        }*/

        /*[TestMethod()]
        public async Task UpdateModelNullColorsListTest()
        {
            Regex regex = new Regex(@"^(Colors list is null)$");
            modelRequest.colors = null;

            try
            {
                await modelController.UpdateModel(modelRequest);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }
        }*/

        /*[TestMethod()]
        public async Task UpdateModellNullSizesListTest()
        {
            Regex regex = new Regex(@"^(Sizes list is null)$");
            modelRequest.sizes = null;

            try
            {
                await modelController.UpdateModel(modelRequest);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }
        }*/

        /*[TestMethod()]
        public async Task UpdateModelNullComponentsListTest()
        {
            Regex regex = new Regex(@"^(Components list is null)$");
            modelRequest.components = null;

            try
            {
                await modelController.UpdateModel(modelRequest);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }
        }*/

        /*[TestMethod()]
        public async Task UpdateModelTestEmptyId()
        {
            Regex regex = new Regex(@"^(ModelId is null or empty)$");
            modelRequest.model.Id = "";

            try
            {
                await modelController.UpdateModel(modelRequest);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /*[TestMethod()]
        public async Task UpdateModelTestNullId()
        {
            Regex regex = new Regex(@"^(ModelId is null or empty)$");
            modelRequest.model.Id = null;

            try
            {
                await modelController.UpdateModel(modelRequest);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /*[TestMethod()]
        public async Task UpdateModelTestEmptyName()
        {
            Regex regex = new Regex(@"^(Name is Null or Empty)$");
            modelRequest.model.Name = "";

            try
            {
                await modelController.UpdateModel(modelRequest);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /*[TestMethod()]
        public async Task UpdateModelTestNullName()
        {
            Regex regex = new Regex(@"^(Name is Null or Empty)$");
            modelRequest.model.Name = null;

            try
            {
                await modelController.UpdateModel(modelRequest);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /*[TestMethod()]
        public async Task UpdateModelTestDeletedAtNotNull()
        {
            Regex regex = new Regex(@"^(This model can't be created or updated because has been desactivated!)$");
            modelRequest.model.DeletedAt = DateTime.Now;

            try
            {
                await modelController.UpdateModel(modelRequest);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/
    }
}