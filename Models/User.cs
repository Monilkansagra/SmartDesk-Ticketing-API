using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Backend_API.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public DateTime? CreatedAt { get; set; }
    [JsonIgnore]
    [ValidateNever]
    public virtual Role? Role { get; set; } 
    [JsonIgnore]
    [ValidateNever]
    public virtual ICollection<Ticket> TicketAssignedToNavigations { get; set; } = new List<Ticket>();
    [JsonIgnore]
    [ValidateNever]
    public virtual ICollection<TicketComment> TicketComments { get; set; } = new List<TicketComment>();
    [JsonIgnore]
    [ValidateNever]
    public virtual ICollection<Ticket> TicketCreatedByNavigations { get; set; } = new List<Ticket>();
    [JsonIgnore]
    [ValidateNever]
    public virtual ICollection<TicketStatusLog> TicketStatusLogs { get; set; } = new List<TicketStatusLog>();
}
