using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyTrace.Models;

public partial class UserRequest
{
    public User User { get; set; }

    public IFormFile? Avatar { get; set; }
}
