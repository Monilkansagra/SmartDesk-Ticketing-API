using Backend_API.Models;
using Backend_API.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_API.Controllers
{
    [Route("tickets")]
    [ApiController]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly SmartDeskDbContext _context;

        public TicketsController(SmartDeskDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Get(
    int page = 1,
    int pageSize = 5,
    string? status = null,
    string? priority = null,
    int? createdBy = null)
        {
            var query = _context.Tickets.AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(t => t.Status == status);

            if (!string.IsNullOrEmpty(priority))
                query = query.Where(t => t.Priority == priority);

            if (createdBy.HasValue)
                query = query.Where(t => t.CreatedBy == createdBy.Value);

            var totalRecords = query.Count();

            var tickets = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                TotalRecords = totalRecords,
                CurrentPage = page,
                PageSize = pageSize,
                Data = tickets
            });
        }


        [HttpPost]
        public IActionResult AddTicket(Ticket ticket)
        {
            if (ticket == null)
                return BadRequest("Invalid ticket data");

            var error = TicketValidation.Validate(ticket);
            if (error != null)
                return BadRequest(error);

            ticket.CreatedAt = DateTime.Now;

            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            return Ok("Ticket created successfully");
        }

        [HttpPatch("{id}/assign")]
        [Authorize(Roles = "MANAGER,SUPPORT")]
        public IActionResult AssignTicket(int id, int userId)
        {
            var ticket = _context.Tickets.Find(id);
            if (ticket == null)
                return NotFound("Ticket not found");

            var user = _context.Users.Find(userId);
            if (user == null)
                return NotFound("User not found");

            if (user.RoleId == 3) 
                return BadRequest("Ticket cannot be assigned to USER role");

            ticket.AssignedTo = userId;
            _context.SaveChanges();

            return Ok("Ticket assigned successfully");
        }

        
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "MANAGER,SUPPORT")]
        public IActionResult UpdateStatus(int id, string newStatus)
        {
            var ticket = _context.Tickets.Find(id);
            if (ticket == null)
                return NotFound("Ticket not found");

            var validFlow = new Dictionary<string, string>
            {
                { "OPEN", "IN_PROGRESS" },
                { "IN_PROGRESS", "RESOLVED" },
                { "RESOLVED", "CLOSED" }
            };

            if (!validFlow.ContainsKey(ticket.Status) ||
                validFlow[ticket.Status] != newStatus)
            {
                return BadRequest("Invalid status transition");
            }

            ticket.Status = newStatus;
            _context.SaveChanges();

            return Ok("Status updated successfully");
        }

        
        [HttpDelete("{id}")]
        [Authorize(Roles = "MANAGER")]
        public IActionResult DeleteTicket(int id)
        {
            var ticket = _context.Tickets.Find(id);
            if (ticket == null)
                return NotFound("Ticket not found");

            _context.Tickets.Remove(ticket);
            _context.SaveChanges();

            return Ok("Ticket deleted successfully");
        }
    }
}