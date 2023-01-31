using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyTrace.Models;

public partial class OrganizationRequest
{
    public Organization Organization { get; set; }

    public IFormFile organizationPhoto { get; set; }

    public IFormFile organizationLogo { get; set; }
}
