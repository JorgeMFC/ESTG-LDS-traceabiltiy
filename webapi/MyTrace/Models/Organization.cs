using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyTrace.Models;

public partial class Organization
{
    public int? Id { get; set; }

    public string Name { get; set; } = null!;

    public string Logo { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string WalletAddress { get; set; } = null!;

    public string? RegexIdClients { get; set; }

    public string? RegexIdOrganizationsAddresses { get; set; }

    public string? RegexIdLots { get; set; }

    public string? RegexIdColors { get; set; }

    public string? RegexIdCoponents { get; set; }

    public string? RegexComponentsType { get; set; }

    public string? RegexIdModels { get; set; }

    public string? RegexIdProviders { get; set; }

    public string? RegexIdSizes { get; set; }

    public string? RegexIdStates { get; set; }

    public string? RegexIdStatesModel { get; set; }

    public string? RegexIdStatesType { get; set; }

    public string? Photo { get; set; }

    [JsonIgnore]
    public virtual ICollection<Client> Clients { get; } = new List<Client>();

    [JsonIgnore]
    public virtual ICollection<Color> Colors { get; } = new List<Color>();

    [JsonIgnore]
    public virtual ICollection<ComponentsType> ComponentsTypes { get; } = new List<ComponentsType>();

    [JsonIgnore]
    public virtual ICollection<OrganizationsAddress> OrganizationsAddresses { get; } = new List<OrganizationsAddress>();

    [JsonIgnore]
    public virtual ICollection<Provider> Providers { get; } = new List<Provider>();

    [JsonIgnore]
    public virtual ICollection<Size> Sizes { get; } = new List<Size>();

    [JsonIgnore]
    public virtual ICollection<Stage> Stages { get; } = new List<Stage>();

    [JsonIgnore]
    public virtual ICollection<StagesModel> StagesModels { get; } = new List<StagesModel>();

    [JsonIgnore]
    public virtual ICollection<User> Users { get; } = new List<User>();
}
