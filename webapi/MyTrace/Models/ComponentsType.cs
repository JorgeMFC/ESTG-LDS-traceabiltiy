using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyTrace.Models;

public partial class ComponentsType
{
    public byte? Id { get; set; }

    public int OrganizationId { get; set; }

    public string ComponentType { get; set; } = null!;

    public DateTime? DeletedAt { get; set; }

    [JsonIgnore]
    public virtual ICollection<Component> Components { get; } = new List<Component>();

    [JsonIgnore]
    public virtual Organization? Organization { get; set; } = null;
}
