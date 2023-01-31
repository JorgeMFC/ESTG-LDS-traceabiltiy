using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTrace.Domain.Exceptions;
using MyTrace.Models;
using MyTrace.Utils;
using System.Text.RegularExpressions;

namespace MyTrace.Controllers.Tests
{
    [TestClass()]
    public class ComponentTypeControllerTests
    {

        /*private readonly MyTraceContext _context = new MyTraceContext();
        ComponentTypeController componentTypeController;
        private ComponentsType componentType = new ComponentsType();

        public ComponentTypeControllerTests()
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

            componentTypeController = new ComponentTypeController(_context, jwtAuthenticationManager);
            componentType.Id = 1;
            componentType.OrganizationId = 1;
            componentType.ComponentType = "Sola";
        }*/

        /**<summary>
         * Test update null componentType 
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddComponentTypeTestNull()
        {
            Regex regex = new Regex(@"^(componentsType is null!)$");
            componentType = null;

            try
            {
                await componentTypeController.AddComponentType(componentType);
            }
            catch (ComponentsTypeArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test add componentType with empty ComponentType
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddComponentTypeTestComponentTypeIsEmpty()
        {
            Regex regex = new Regex(@"^(ComponentType is Null or Empty)$");
            componentType.ComponentType = "";

            try
            {
                await componentTypeController.AddComponentType(componentType);
            }
            catch (ComponentsTypeArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test add componentType with null ComponentType
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddComponentTypeTestComponentTypeIsNull()
        {
            Regex regex = new Regex(@"^(ComponentType is Null or Empty)$");
            componentType.ComponentType = null;

            try
            {
                await componentTypeController.AddComponentType(componentType);
            }
            catch (ComponentsTypeArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test update componentType with empty ComponentType
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateComponentTypeTestComponentTypeIsEmpty()
        {
            Regex regex = new Regex(@"^(ComponentType is Null or Empty)$");
            componentType.ComponentType = "";

            try
            {
                await componentTypeController.UpdateComponentType(componentType);
            }
            catch (ComponentsTypeArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test update componentType with null ComponentType
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateComponentTypeTestComponentTypeIsNull()
        {
            Regex regex = new Regex(@"^(ComponentType is Null or Empty)$");
            componentType.ComponentType = null;

            try
            {
                await componentTypeController.UpdateComponentType(componentType);
            }
            catch (ComponentsTypeArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test update null componentType 
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateComponentTypeTestNull()
        {
            Regex regex = new Regex(@"^(componentsType is null!)$");
            componentType = null;

            try
            {
                await componentTypeController.UpdateComponentType(componentType);
            }
            catch (ComponentsTypeArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/
    }
}