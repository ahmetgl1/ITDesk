using ITDeskServer.Models;

namespace ITDeskServer.DTOs.Requests;

public class TicketDetailRequestDto
{

    public Guid TicketId { get; set; }
    public string? Content { get; set;}
    public Guid AppUserId { get; set;}

}
