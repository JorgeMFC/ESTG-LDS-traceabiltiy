
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyTrace.Models;

public partial class Color
{
    public string Id { get; set; } = null!;

    public int OrganizationId { get; set; }

    public string Color1 { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<ModelsSizesColor> ModelsSizesColors { get; } = new List<ModelsSizesColor>();

    [JsonIgnore]
    public virtual Organization? Organization { get; set; } = null;
}
