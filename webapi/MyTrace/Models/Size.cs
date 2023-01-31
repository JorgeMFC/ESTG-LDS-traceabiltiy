using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace MyTrace.Models;

public partial class Size
{
    public string Id { get; set; } = null!;

    public int OrganizationId { get; set; }

    public string Size1 { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<ModelsSizesColor> ModelsSizesColors { get; } = new List<ModelsSizesColor>();

    [JsonIgnore]
    public virtual Organization? Organization { get; set; } = null;
}
