using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace MyTrace.Models;

public partial class Stage
{
    public string Id { get; set; } = null!;

    public int OrganizationId { get; set; }

    public string StageName { get; set; } = null!;

    public string StagesTypeId { get; set; } = null!;

    public string StageDescription { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<LotsStage> LotsStages { get; } = new List<LotsStage>();

    [JsonIgnore]
    public virtual Organization? Organization { get; set; } = null;

    [JsonIgnore]
    public virtual ICollection<StagesModelStage> StagesModelStages { get; } = new List<StagesModelStage>();

    [JsonIgnore]
    public virtual StagesType? StagesType { get; set; } = null;
}
