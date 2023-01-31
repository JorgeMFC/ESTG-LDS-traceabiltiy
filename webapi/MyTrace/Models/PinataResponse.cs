namespace MyTrace.Models
{
    public class PinataResponse
    {
        public string IpfsHash { get; set; }
        public int PinSize { get; set; }
        public DateTime Timestamp { get; set; }
        public bool isDuplicate { get; set; }
    }
}
