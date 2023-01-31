using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTrace.Domain.Exceptions;
using MyTrace.Models;
using MyTrace.Utils;
using System.Text.RegularExpressions;

namespace MyTrace.Controllers.Tests
{
    [TestClass()]
    public class ProviderControllerTests
    {
        /*private readonly MyTraceContext _context = new MyTraceContext();
        ProviderController providerController;
        ProviderAndComponents providerAndComponents = new ProviderAndComponents();

        public ProviderControllerTests()
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

            providerController = new ProviderController(_context, jwtAuthenticationManager);
            Provider provider = new Provider();
            List<Component> components = new List<Component>();

            provider.Id = "PR1";
            provider.OrganizationId = 1;
            provider.Name = "Luis";
            provider.Email = "luis@gmail.com";
            provider.DeletedAt = null;
            provider.CreatedAt = DateTime.Now;

            providerAndComponents.provider = provider;
            providerAndComponents.components = components;
        }*/

        /** <summary>
        * Test add null provider
        * </summary>
        */
        /*[TestMethod()]
        public async Task AddProviderTestNull()
        {
            Regex regex = new Regex(@"^(provider is null!)$");
            providerAndComponents.provider = null;

            try
            {
                await providerController.AddProvider(providerAndComponents);
            }
            catch (ProviderArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
        * Test add provider with null components
        * </summary>
        */
        /*[TestMethod()]
        public async Task AddProviderTestNullComponents()
        {
            Regex regex = new Regex(@"^(Components list is null)$");
            providerAndComponents.components = null;

            try
            {
                await providerController.AddProvider(providerAndComponents);
            }
            catch (ProviderArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add provider with an empty name
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddProviderTestEmptyName()
        {
            Regex regex = new Regex(@"^(Name is Null or Empty)$");
            providerAndComponents.provider.Name = "";

            try
            {
                await providerController.AddProvider(providerAndComponents);
            }
            catch (ProviderArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add provider with a null name
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddProviderTestNullName()
        {
            Regex regex = new Regex(@"^(Name is Null or Empty)$");
            providerAndComponents.provider.Name = null;

            try
            {
                await providerController.AddProvider(providerAndComponents);
            }
            catch (ProviderArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add provider with an empty email
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddProviderTestEmptyEmail()
        {
            Regex regex = new Regex(@"^(Provider email is Null or Empty)$");
            providerAndComponents.provider.Email = "";

            try
            {
                await providerController.AddProvider(providerAndComponents);
            }
            catch (ProviderArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add provider with a null email
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddProviderTestNullEmail()
        {
            Regex regex = new Regex(@"^(Provider email is Null or Empty)$");
            providerAndComponents.provider.Email = null;

            try
            {
                await providerController.AddProvider(providerAndComponents);
            }
            catch (ProviderArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add provider with an invalid email, that is email does not contain @ annotation 
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddProviderTestInvalidEmail()
        {
            Regex regex = new Regex(@"^(Provider email format is not valid!)$");
            providerAndComponents.provider.Email = "luisgmail.com";

            try
            {
                await providerController.AddProvider(providerAndComponents);
            }
            catch (ProviderArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add provider with a deletedAt Field not as null, that is add an user that's already desactivated before being added
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddProviderWithInvalidDeletedFieldTest()
        {
            Regex regex = new Regex(@"^(This provider can't be created or updated because has been desactivated!)$");
            providerAndComponents.provider.DeletedAt = DateTime.Now;

            try
            {
                await providerController.AddProvider(providerAndComponents);
            }
            catch (ProviderArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
        * Test update null provider
        * </summary>
        */
        /*[TestMethod()]
        public async Task UpdateProviderTestNull()
        {
            Regex regex = new Regex(@"^(provider is null!)$");
            providerAndComponents.provider = null;

            try
            {
                await providerController.UpdateProvider(providerAndComponents);
            }
            catch (ProviderArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
       * Test update provider with null components
       * </summary>
       */
        /*[TestMethod()]
        public async Task UpdateProviderTestNullComponents()
        {
            Regex regex = new Regex(@"^(Components list is null)$");
            providerAndComponents.components = null;

            try
            {
                await providerController.UpdateProvider(providerAndComponents);
            }
            catch (ProviderArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/


        /** <summary>
         * Test update provider with an empty name
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateProviderTestEmptyName()
        {
            Regex regex = new Regex(@"^(Name is Null or Empty)$");
            providerAndComponents.provider.Name = "";

            try
            {
                await providerController.UpdateProvider(providerAndComponents);
            }
            catch (ProviderArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update provider with a null name
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateProviderTestNullName()
        {
            Regex regex = new Regex(@"^(Name is Null or Empty)$");
            providerAndComponents.provider.Name = null;

            try
            {
                await providerController.UpdateProvider(providerAndComponents);
            }
            catch (ProviderArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update provider with an empty email
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateProviderTestEmptyEmail()
        {
            Regex regex = new Regex(@"^(Provider email is Null or Empty)$");
            providerAndComponents.provider.Email = "";

            try
            {
                await providerController.UpdateProvider( providerAndComponents);
            }
            catch (ProviderArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update provider with a null email
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateProviderTestNullEmail()
        {
            Regex regex = new Regex(@"^(Provider email is Null or Empty)$");
            providerAndComponents.provider.Email = null;

            try
            {
                await providerController.UpdateProvider(providerAndComponents);
            }
            catch (ProviderArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update provider with an invalid email, that is email does not contain @ annotation 
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateProviderTestInvalidEmail()
        {
            Regex regex = new Regex(@"^(Provider email format is not valid!)$");
            providerAndComponents.provider.Email = "luisgmail.com";

            try
            {
                await providerController.UpdateProvider(providerAndComponents);
            }
            catch (ProviderArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update provider with a deletedAt Field not as null, that is add an user that's already desactivated before being added
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateProviderWithInvalidDeletedFieldTest()
        {
            Regex regex = new Regex(@"^(This provider can't be created or updated because has been desactivated!)$");
            providerAndComponents.provider.DeletedAt = DateTime.Now;

            try
            {
                await providerController.UpdateProvider(providerAndComponents);
            }
            catch (ProviderArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test add provider with a invalid created account date, that is the created account date is greater than current date
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddProviderdTestInvalidCreatedDate()
        {
            Regex regex = new Regex(@"^(Provider account createdDate cannot be greater than current date)$");
            providerAndComponents.provider.CreatedAt = DateTime.Now.AddDays(1);

            try
            {
                await providerController.AddProvider(providerAndComponents);
            }
            catch (ProviderArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /** <summary>
         * Test update provider with a invalid created account date, that is the update account date is greater than current date
         * </summary>
         */
        /*[TestMethod()]
        public async Task UpdateProviderTestInvalidCreatedDate()
        {
            Regex regex = new Regex(@"^(Provider account createdDate cannot be greater than current date)$");
            providerAndComponents.provider.CreatedAt = DateTime.Now.AddDays(1);

            try
            {
                await providerController.AddProvider(providerAndComponents);
            }
            catch (ProviderArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/
    }
}