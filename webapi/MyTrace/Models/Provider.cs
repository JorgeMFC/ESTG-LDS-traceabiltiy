using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace MyTrace.Models;

public partial class Provider
{
    public string Id { get; set; } = null!;

    public int OrganizationId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    [JsonIgnore]
    public virtual ICollection<Component> Components { get; } = new List<Component>();

    [JsonIgnore]
    public virtual Organization? Organization { get; set; } = null;
}
