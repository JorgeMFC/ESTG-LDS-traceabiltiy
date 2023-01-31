namespace MyTrace.Models
{
    public class CLientAndClientAddress
    {
        public Client client { get; set; } = null!;

        public List<ClientsAddress> clientsAddressesList { get; set; } = null!;
    }
}
