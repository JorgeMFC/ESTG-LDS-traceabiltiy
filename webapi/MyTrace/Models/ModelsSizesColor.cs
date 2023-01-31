using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace MyTrace.Models;

public partial class ModelsSizesColor
{
    public string ModelId { get; set; } = null!;

    public string ColorId { get; set; } = null!;

    public string SizeId { get; set; } = null!;

    public int OrganizationId { get; set; }

    public DateTime? DeletedAt { get; set; }

    [JsonIgnore]
    public virtual Color? Color { get; set; } = null;

    [JsonIgnore]
    public virtual ICollection<Lot> Lots { get; } = new List<Lot>();

    [JsonIgnore]
    public virtual Model? Model { get; set; } = null;

    [JsonIgnore]
    public virtual Size? Size { get; set; } = null;
}
