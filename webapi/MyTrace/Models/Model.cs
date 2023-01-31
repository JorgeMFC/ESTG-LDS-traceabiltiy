using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyTrace.Models;

public partial class Model
{
    public string Id { get; set; } = null!;

    public int OrganizationId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? DeletedAt { get; set; }

    public string StagesModelId { get; set; } = null!;

    public string ModelPhoto { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Lot> Lots { get; } = new List<Lot>();

    [JsonIgnore]
    public virtual ICollection<ModelsComponent> ModelsComponents { get; } = new List<ModelsComponent>();

    [JsonIgnore]
    public virtual ICollection<ModelsSizesColor> ModelsSizesColors { get; } = new List<ModelsSizesColor>();

    [JsonIgnore]
    public virtual StagesModel? StagesModel { get; set; } = null;
}
