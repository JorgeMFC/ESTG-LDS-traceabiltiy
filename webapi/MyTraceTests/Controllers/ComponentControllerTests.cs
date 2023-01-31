using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTrace.Domain.Exceptions;
using MyTrace.Models;
using MyTrace.Utils;
using System.Text.RegularExpressions;

namespace MyTrace.Controllers.Tests
{
    [TestClass()]
    public class ComponentControllerTests
    {
        /*private readonly MyTraceContext _context = new MyTraceContext();
        ComponentController componentController;
        private Component component = new Component();

        public ComponentControllerTests()
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

            componentController = new ComponentController(_context, jwtAuthenticationManager);
            component.Id = "SO0004";
            component.OrganizationId = 1;
            component.ComponentsTypeId = 1;
            component.ProviderId = null;
            component.DeletedAt = null;
        }*/

        /**<summary>
         * Test add null component
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddComponentTestNull()
        {
            Regex regex = new Regex(@"^(component is null)$");
            component = null;

            try
            {
                await componentController.AddComponent(component);
            }
            catch (ComponentArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test add component with a deletedAt Field not as null, that is add an component that's already desactivated before being added
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddComponentTestDeletedAtIsNotNull()
        {
            Regex regex = new Regex(@"^(This is component can't be created or updated because has been desactivated!)$");
            component.DeletedAt = DateTime.Now;

            try
            {
                await componentController.AddComponent(component);
            }
            catch (ComponentArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();

        }*/

        /**<summary>
         * Test add component with empty Id
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddComponentTestEmptyId()
        {
            Regex regex = new Regex(@"^(ComponentId is null or empty)$");
            component.Id = "";

            try
            {
                await componentController.AddComponent(component);
            }
            catch (ComponentArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test add component with null Id
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddComponentTestNullId()
        {
            Regex regex = new Regex(@"^(ComponentId is null or empty)$");
            component.Id = null;

            try
            {
                await componentController.AddComponent(component);
            }
            catch (ComponentArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test update null component
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateComponentTestNull()
        {
            Regex regex = new Regex(@"^(component is null)$");
            component = null;

            try
            {
                await componentController.UpdateComponent(component);
            }
            catch (ComponentArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test update component with a deletedAt Field not as null, that is update an component that's already desactivated before being added
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateComponentTestDeletedAtIsNotNull()
        {
            Regex regex = new Regex(@"^(This is component can't be created or updated because has been desactivated!)$");
            component.DeletedAt = DateTime.Now;

            try
            {
                await componentController.UpdateComponent(component);
            }
            catch (ComponentArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();

        }*/

        /**<summary>
         * Test update component with empty Id
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateComponentTestEmptyId()
        {
            Regex regex = new Regex(@"^(ComponentId is null or empty)$");
            component.Id = "";

            try
            {
                await componentController.UpdateComponent(component);
            }
            catch (ComponentArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test update component with null Id
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateComponentTestNullId()
        {
            Regex regex = new Regex(@"^(ComponentId is null or empty)$");
            component.Id = null;

            try
            {
                await componentController.UpdateComponent(component);
            }
            catch (ComponentArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/
    }
}