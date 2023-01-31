using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace MyTrace.Models;

public partial class Lot
{
    public string Id { get; set; } = null!;

    public int OrganizationId { get; set; }

    public string ModelId { get; set; } = null!;

    public string ModelColorId { get; set; } = null!;

    public string ModelSizeId { get; set; } = null!;

    public string ClientId { get; set; } = null!;

    public string ClientAddressId { get; set; } = null!;

    public string OrganizationAddressId { get; set; } = null!;

    public DateTime? DeliveryDate { get; set; }

    public short LotSize { get; set; }

    public string? Hash { get; set; } = null!;

    public string StagesModelId { get; set; } = null!;

    public DateTime? CanceledAt { get; set; }

    [JsonIgnore]
    public virtual ClientsAddress? ClientsAddress { get; set; } = null;

    [JsonIgnore]
    public virtual ICollection<LotsStage> LotsStages { get; } = new List<LotsStage>();

    [JsonIgnore]
    public virtual Model? Model { get; set; } = null;

    [JsonIgnore]
    public virtual ModelsSizesColor? ModelsSizesColor { get; set; } = null;

    [JsonIgnore]
    public virtual OrganizationsAddress? Organization { get; set; } = null;

    [JsonIgnore]
    public virtual StagesModel? StagesModel { get; set; } = null;
}
