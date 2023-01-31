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
    public class ColorsSizesControllerTests
    {
        /*private readonly MyTraceContext _context = new MyTraceContext();
        ColorsSizesController colorsSizesController;
        private Color color = new Color();
        private Size size = new Size();

        public ColorsSizesControllerTests()
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

            colorsSizesController = new ColorsSizesController(_context, jwtAuthenticationManager);
            color.Id = "CO001";
            color.OrganizationId = 1;
            color.Color1 = "Vermelho";

            size.Id = "SI001";
            size.OrganizationId = 1;
            size.Size1 = "42";
        }*/

        /**<summary>
         * Test update null color 
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddColorTestNull()
        {
            Regex regex = new Regex(@"^(color is null!)$");
            color = null;

            try
            {
                await colorsSizesController.AddColor(color);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test update null size 
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddSizeTestNull()
        {
            Regex regex = new Regex(@"^(size is null!)$");
            size = null;

            try
            {
                await colorsSizesController.AddSize(size);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test add color with empty color
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddColorTestColorIsEmpty()
        {
            Regex regex = new Regex(@"^(Color is Null or Empty)$");
            color.Color1 = "";

            try
            {
                await colorsSizesController.AddColor(color);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test add size with empty size
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddSizeTestSizeIsEmpty()
        {
            Regex regex = new Regex(@"^(Size is Null or Empty)$");
            size.Size1 = "";

            try
            {
                await colorsSizesController.AddSize(size);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test add color with null color
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddColorTestColorIsNull()
        {
            Regex regex = new Regex(@"^(Color is Null or Empty)$");
            color.Color1 = null;

            try
            {
                await colorsSizesController.AddColor(color);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test add size with null size
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddSizeTestSizeIsNull()
        {
            Regex regex = new Regex(@"^(Size is Null or Empty)$");
            size.Size1 = null;

            try
            {
                await colorsSizesController.AddSize(size);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test add color with null colorId
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddColorTestColorIdIsNull()
        {
            Regex regex = new Regex(@"^(ColorId is Null or Empty)$");
            color.Id = null;

            try
            {
                await colorsSizesController.AddColor(color);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test add size with null sizeId
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddSizeTestSizeIdIsNull()
        {
            Regex regex = new Regex(@"^(SizeId is Null or Empty)$");
            size.Id = null;

            try
            {
                await colorsSizesController.AddSize(size);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test add color with empty colorId
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddColorTestColorIdIsEmpty()
        {
            Regex regex = new Regex(@"^(ColorId is Null or Empty)$");
            color.Id = "";

            try
            {
                await colorsSizesController.AddColor(color);
            }
            catch (ModelArgumentException ex)
            {
                StringAssert.Matches(ex.Message, regex);
                return;
            }

            Assert.Fail();
        }*/

        /**<summary>
         * Test add size with empty sizeId
         * </summary>
         */
        /*[TestMethod()]
        public async Task AddSizeTestSizeIdIsEmpty()
        {
            Regex regex = new Regex(@"^(SizeId is Null or Empty)$");
            size.Id = "";

            try
            {
                await colorsSizesController.AddSize(size);
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