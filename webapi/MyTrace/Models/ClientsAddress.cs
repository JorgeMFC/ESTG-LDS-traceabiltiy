using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyTrace.Models;

public partial class ClientsAddress
{
    public string Id { get; set; } = null!;

    public string ClientId { get; set; } = null!;

    public int OrganizationId { get; set; }

    public string Address { get; set; } = null!;

    public string Zipcode { get; set; } = null!;

    public DateTime? DeletedAt { get; set; }

    [JsonIgnore]
    public virtual Client? Client { get; set; } = null;

    [JsonIgnore]
    public virtual ICollection<Lot> Lots { get; } = new List<Lot>();
}
