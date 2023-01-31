using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace MyTrace.Models;

public partial class UsersTypeRoute
{
    public byte UserTypeId { get; set; }

    public string Route { get; set; } = null!;

    public string Permissions { get; set; } = null!;

    [JsonIgnore]
    public virtual Route? RoutesNavigation { get; set; } = null;

    [JsonIgnore]
    public virtual UsersType? UserType { get; set; } = null;
}
