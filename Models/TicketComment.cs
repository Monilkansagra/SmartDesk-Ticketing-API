using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Backend_API.Models;

public partial class TicketComment
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public int UserId { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
    [JsonIgnore]
    public virtual Ticket Ticket { get; set; } = null!;
    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
