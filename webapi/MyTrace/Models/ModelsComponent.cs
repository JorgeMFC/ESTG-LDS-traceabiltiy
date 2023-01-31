using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace MyTrace.Models;

public partial class ModelsComponent
{
    public string ModelId { get; set; } = null!;

    public string ComponentsId { get; set; } = null!;

    public int OrganizationId { get; set; }

    public int Amount { get; set; }

    [JsonIgnore]
    public virtual Component? Component { get; set; } = null;

    [JsonIgnore]
    public virtual Model? Model { get; set; } = null;
}
