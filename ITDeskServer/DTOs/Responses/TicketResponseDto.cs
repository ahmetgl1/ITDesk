namespace ITDeskServer.DTOs.Responses;

public sealed record TicketResponseDto

    ( 
       Guid Id,
      string? Subject,
     // string? Summary,
      bool? IsOpen,
      DateTime? CreatedDate
    );