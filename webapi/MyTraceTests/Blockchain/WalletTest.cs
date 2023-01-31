using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTrace.Blockchain;

namespace MyTraceTests.Blockchain
{
    [TestClass()]
    public class WalletTest
    {
        [TestMethod()]
        public void IsValidEthereumAddressTestNull()
        {
            string wallet = null;
            if (!Wallet.IsValidEthereumAddress(wallet))
            {
                return;
            }

            Assert.Fail();
        }

        [TestMethod()]
        public void IsValidEthereumAddressTestEmpty()
        {
            string wallet = "";
            if (!Wallet.IsValidEthereumAddress(wallet))
            {
                return;
            }

            Assert.Fail();
        }

        [TestMethod()]
        public void IsValidEthereumAddressTestInvalid()
        {
            //bitcoin wallet
            string wallet = "1BoatSLRHtKNngkdXEeobR76b53LETtpyT";
            if (!Wallet.IsValidEthereumAddress(wallet))
            {
                return;
            }

            Assert.Fail();
        }

        [TestMethod()]
        public void IsValidEthereumAddressTestValid()
        {
            string wallet = "0xe21d837cd1437305632ac1660a94c64b1ecd3151";
            if (Wallet.IsValidEthereumAddress(wallet))
            {
                return;
            }

            Assert.Fail();
        }

        [TestMethod()]
        public void VerifySignatureTestNull()
        {
            if (!Wallet.VerifySignature(
                    null,
                    null,
                    null
               )
            )
            {
                return;
            }

            Assert.Fail();
        }
    }
}
