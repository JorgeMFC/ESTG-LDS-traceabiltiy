using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace MyTrace.Models;

public partial class Route
{
    public string route { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<UsersTypeRoute> UsersTypeRoutes { get; } = new List<UsersTypeRoute>();
}
