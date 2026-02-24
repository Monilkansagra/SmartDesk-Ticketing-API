using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Backend_API.Models;

public partial class TicketStatusLog
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public string OldStatus { get; set; } = null!;

    public string NewStatus { get; set; } = null!;

    public int ChangedBy { get; set; }

    public DateTime? ChangedAt { get; set; }
    [JsonIgnore]
    public virtual User ChangedByNavigation { get; set; } = null!;
    [JsonIgnore]
    public virtual Ticket Ticket { get; set; } = null!;
}
