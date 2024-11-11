using System;
using Core.Enums;

namespace Core.Entities;

public class ExportRequest
{
    public int ExportRequestId { get; set; }
    public string UserId { get; set; }  = string.Empty;
    public ExportFormat Format { get; set; } 
    public DateTime RequestedAt { get; set; }
    public DateTime? CompletedAt { get; set; } 
    public AppUser User { get; set; } = null!;
}


