using Backend_API.Models;

namespace Backend_API.Validation
{
    public static class TicketValidation
    {
        public static string? Validate(Ticket ticket)
        {
            if (string.IsNullOrWhiteSpace(ticket.Title) || ticket.Title.Length < 5)
                return "Title must be minimum 5 characters";

            if (string.IsNullOrWhiteSpace(ticket.Description) || ticket.Description.Length < 10)
                return "Description must be minimum 10 characters";

            var validPriority = new[] { "LOW", "MEDIUM", "HIGH" };
            if (string.IsNullOrEmpty(ticket.Priority) || !validPriority.Contains(ticket.Priority))
                return "Priority must be LOW, MEDIUM or HIGH";

            var validStatus = new[] { "OPEN", "IN_PROGRESS", "RESOLVED", "CLOSED" };
            if (string.IsNullOrEmpty(ticket.Status) || !validStatus.Contains(ticket.Status))
                return "Invalid status value";

            return null;
        }
    }
}