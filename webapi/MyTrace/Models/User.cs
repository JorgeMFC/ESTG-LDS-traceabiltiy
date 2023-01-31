using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyTrace.Models;

public partial class User
{
    public string WalletAddress { get; set; } = null!;

    public long Nonce { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public DateTime? BirthDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? OrganizationId { get; set; }

    public byte UserTypeId { get; set; }

    public string? Avatar { get; set; }

    [JsonIgnore]
    public virtual Organization? Organization { get; set; }

    [JsonIgnore]
    public virtual UsersType? UserType { get; set; } = null;

    public static implicit operator User(List<User> v)
    {
        throw new NotImplementedException();
    }
}
