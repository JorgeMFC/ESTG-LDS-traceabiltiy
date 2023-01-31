using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace MyTrace.Models;

public partial class LotsStage
{
    public string LotId { get; set; } = null!;

    public int OrganizationId { get; set; }

    public string StageId { get; set; } = null!;

    public string? Hash { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }
    [JsonIgnore]
    public virtual Lot? Lot { get; set; } = null;
    [JsonIgnore]
    public virtual Stage? Stage { get; set; } = null;
}
