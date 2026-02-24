using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Backend_API.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Status { get; set; }

    public string? Priority { get; set; }

    public int CreatedBy { get; set; }

    public int? AssignedTo { get; set; }

    public DateTime? CreatedAt { get; set; }
    [JsonIgnore]
    [ValidateNever]
    public virtual User? AssignedToNavigation { get; set; }
    [JsonIgnore]
    [ValidateNever]
    public virtual User? CreatedByNavigation { get; set; } 
    [JsonIgnore]
    [ValidateNever]
    public virtual ICollection<TicketComment> TicketComments { get; set; } = new List<TicketComment>();
    [JsonIgnore]
    [ValidateNever]
    public virtual ICollection<TicketStatusLog> TicketStatusLogs { get; set; } = new List<TicketStatusLog>();
}
