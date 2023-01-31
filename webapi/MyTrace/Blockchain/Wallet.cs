using Nethereum.Signer;
using Nethereum.Util;

namespace MyTrace.Blockchain
{
    public class Wallet
    {
        public static Boolean VerifySignature(string address, string mensage, string signature)
        {
            var signer = new EthereumMessageSigner();
            try
            {
                return signer.EncodeUTF8AndEcRecover(mensage, signature).Equals(address, StringComparison.CurrentCultureIgnoreCase);
            }
            catch (FormatException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Boolean IsValidEthereumAddress(string address)
        {
            var AddressUtil = new AddressUtil();

            return AddressUtil.IsValidEthereumAddressHexFormat(address);
        }
    }
}
