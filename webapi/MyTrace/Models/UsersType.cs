using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace MyTrace.Models;

public partial class UsersType
{
    public byte Id { get; set; }

    public string UserType { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<User> Users { get; } = new List<User>();

    [JsonIgnore]
    public virtual ICollection<UsersTypeRoute> UsersTypeRoutes { get; } = new List<UsersTypeRoute>();
}
