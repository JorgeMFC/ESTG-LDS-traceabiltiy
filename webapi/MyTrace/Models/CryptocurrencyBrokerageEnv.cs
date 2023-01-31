namespace MyTrace.Models
{
    public class CryptocurrencyBrokerageEnv
    {
        public List<string> Url { get; set; }
        public int ChainID { get; set; }
        public string Wallet { get; set; }
        public string WalletPrivateKey { get; set; }
        public string ContractAddress { get; set; }
    }
}
