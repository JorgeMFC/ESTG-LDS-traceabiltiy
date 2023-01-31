using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace MyTrace.Models;

public partial class StagesModelStage
{
    public string StagesModelId { get; set; } = null!;

    public string StagesId { get; set; } = null!;

    public int OrganizationId { get; set; }

    public byte Position { get; set; }

    [JsonIgnore]
    public virtual Stage? Stage { get; set; } = null;

    [JsonIgnore]
    public virtual StagesModel? StagesModel { get; set; } = null;
}
