using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace MyTrace.Models;

public partial class StagesModel
{
    public string Id { get; set; } = null!;

    public int OrganizationId { get; set; }

    public string StagesModelName { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Lot> Lots { get; } = new List<Lot>();

    [JsonIgnore]
    public virtual ICollection<Model> Models { get; } = new List<Model>();

    [JsonIgnore]
    public virtual Organization? Organization { get; set; } = null;

    [JsonIgnore]
    public virtual ICollection<StagesModelStage> StagesModelStages { get; } = new List<StagesModelStage>();
}
