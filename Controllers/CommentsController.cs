using Backend_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend_API.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly SmartDeskDbContext _context;

        public CommentsController(SmartDeskDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("tickets/{ticketId}/comments")]
        public IActionResult GetComments(int ticketId)
        {
            var ticket = _context.Tickets.Find(ticketId);

            if (ticket == null)
                return NotFound("Ticket not found");

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var role = User.FindFirst(ClaimTypes.Role).Value;

            if (role != "MANAGER" &&
                !(role == "SUPPORT" && ticket.AssignedTo == userId) &&
                !(role == "USER" && ticket.CreatedBy == userId))
            {
                return Forbid();
            }

            var comments = _context.TicketComments
                .Where(c => c.TicketId == ticketId)
                .ToList();

            return Ok(comments);
        }

        
        [HttpPost("tickets/{ticketId}/comments")]
        public IActionResult AddComment(int ticketId, TicketComment model)
        {
            var ticket = _context.Tickets.Find(ticketId);

            if (ticket == null)
                return NotFound("Ticket not found");

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var role = User.FindFirst(ClaimTypes.Role).Value;

            if (role != "MANAGER" &&
                !(role == "SUPPORT" && ticket.AssignedTo == userId) &&
                !(role == "USER" && ticket.CreatedBy == userId))
            {
                return Forbid();
            }

            var comment = new TicketComment
            {
                TicketId = ticketId,
                UserId = userId,
                Comment = model.Comment,
                CreatedAt = DateTime.Now
            };

            _context.TicketComments.Add(comment);
            _context.SaveChanges();

            return Ok(comment);
        }

        
        [HttpPatch("comments/{id}")]
        public IActionResult UpdateComment(int id, [FromBody] string updatedComment)
        {
            var comment = _context.TicketComments.Find(id);

            if (comment == null)
                return NotFound("Comment not found");

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var role = User.FindFirst(ClaimTypes.Role).Value;

            if (role != "MANAGER" && comment.UserId != userId)
                return Forbid();

            comment.Comment = updatedComment;
            _context.SaveChanges();

            return Ok("Comment updated successfully");
        }

        
        [HttpDelete("comments/{id}")]
        public IActionResult DeleteComment(int id)
        {
            var comment = _context.TicketComments.Find(id);

            if (comment == null)
                return NotFound("Comment not found");

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var role = User.FindFirst(ClaimTypes.Role).Value;

            if (role != "MANAGER" && comment.UserId != userId)
                return Forbid();

            _context.TicketComments.Remove(comment);
            _context.SaveChanges();

            return Ok("Comment deleted successfully");
        }
    }
}