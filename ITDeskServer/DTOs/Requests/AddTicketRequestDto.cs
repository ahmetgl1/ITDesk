namespace ITDeskServer.DTOs.Requests;

public sealed  record AddTicketRequestDto

    (
      string Subject,
      string Summary,
      List<IFormFile> Files
    );