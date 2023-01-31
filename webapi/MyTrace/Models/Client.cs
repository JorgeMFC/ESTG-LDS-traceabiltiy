using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyTrace.Models;

public partial class Client
{
    public string Id { get; set; } = null!;

    public int OrganizationId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? DeletedAt { get; set; }

    [JsonIgnore]
    public virtual ICollection<ClientsAddress> ClientsAddresses { get; } = new List<ClientsAddress>();

    [JsonIgnore]
    public virtual Organization? Organization { get; set; } = null;
}
