using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace MyTrace.Models;

public partial class StagesType
{
    public string Id { get; set; } = null!;

    public string StageType { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Stage> Stages { get; } = new List<Stage>();
}
