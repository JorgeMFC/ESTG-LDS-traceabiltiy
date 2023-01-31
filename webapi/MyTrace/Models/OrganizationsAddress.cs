using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace MyTrace.Models;

public partial class OrganizationsAddress
{
    public string Id { get; set; } = null!;

    public int OrganizationId { get; set; }

    public string Address { get; set; } = null!;

    public string Zipcode { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Lot> Lots { get; } = new List<Lot>();

    [JsonIgnore]
    public virtual Organization? Organization { get; set; } = null;
}
