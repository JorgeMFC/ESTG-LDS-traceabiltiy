using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace MyTrace.Models;

public partial class Component
{
    public string Id { get; set; } = null!;

    public int OrganizationId { get; set; }

    public byte ComponentsTypeId { get; set; }

    public string? ProviderId { get; set; }

    public DateTime? DeletedAt { get; set; }

    [JsonIgnore]
    public virtual ComponentsType? ComponentsType { get; set; } = null;

    [JsonIgnore]
    public virtual ICollection<ModelsComponent> ModelsComponents { get; } = new List<ModelsComponent>();

    [JsonIgnore]
    public virtual Provider? Provider { get; set; }
}
