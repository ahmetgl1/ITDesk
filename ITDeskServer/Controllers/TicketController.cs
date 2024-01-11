using ITDeskServer.Abstraction;
using ITDeskServer.Context;
using ITDeskServer.DTOs.Requests;
using ITDeskServer.DTOs.Responses;
using ITDeskServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ITDeskServer.Controllers;

public class TicketController : ApiController
{

    private readonly ApplicationDbContext _applicationDbContext;

    public TicketController(ApplicationDbContext applicationDbContext)
    {

        _applicationDbContext = applicationDbContext;
    }


    [HttpPost]
    public IActionResult CreateTicket([FromForm] AddTicketRequestDto addTicketRequestDto)
    {

        string? userId = HttpContext.User.Claims.Where(o => o.Type == "UserId").Select(p => p.Value).FirstOrDefault();


        if (userId is null)
        {
            return NotFound(new { Message = "Kullanıcı Bulunamadı" });
        }


        Ticket ticket = new()
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            AppUserId = Guid.Parse(userId),
            IsOpen = true,
            Subject = addTicketRequestDto.Subject
        };

        if (addTicketRequestDto.Files is not null)
        {

            ticket.FileUrls = new();


            foreach (var file in addTicketRequestDto.Files)
            {

                string fileFormat = file.FileName.Substring(file.FileName.LastIndexOf('.')); //ex: .jpg
                string fileName = Guid.NewGuid().ToString() + fileFormat; // ex:  2432421-324324.jpg



                using (var stream = System.IO.File.Create(@"G:\Projeler\ITDesk\ITDeskClient\src\assets\files\" + fileName))
                {

                    file.CopyTo(stream);
                }


                TicketFile ticketFile = new()
                {

                    Id = Guid.NewGuid(),
                    FileUrl = fileName,
                    TicketId = ticket.Id
                };

                if (ticket.FileUrls is not null)
                {
                    ticket.FileUrls.Add(ticketFile);
                }

            }

        }


        TicketDetail ticketDetail = new()
        {
            Id = Guid.NewGuid(),
            TicketId = ticket.Id,
            AppUserId = Guid.Parse(userId),
            Content = addTicketRequestDto.Summary,
            CreatedAt = ticket.CreatedAt
        };






        _applicationDbContext.Tickets.Add(ticket);



       // _applicationDbContext.Add(ticket);

        _applicationDbContext.Add(ticketDetail);

        _applicationDbContext.SaveChanges();


        return NoContent();
    }



    [HttpPost]
    [EnableQuery]

    public IActionResult GetAllByUserId(GetAllTicketResponseDto responseDto)
    {


        string? userId = HttpContext.User.Claims.Where(o => o.Type == "UserId").Select(s => s.Value).FirstOrDefault();



        if (userId is null)
        {
            return NotFound(new { Message = "Kullanıcı Bulunamadı !" });
        }



        if (responseDto.roles.Contains("Admin"))
        {
            IQueryable<TicketResponseDto> ticketResponse = _applicationDbContext.Tickets
                        .Select(x => new TicketResponseDto(x.Id, x.Subject, x.IsOpen, x.CreatedAt))

                        .AsQueryable();

            return Ok(ticketResponse);



        }
        else
        {

        IQueryable<TicketResponseDto> ticketResponse = _applicationDbContext.Tickets
                .Where(o => o.AppUserId == Guid.Parse(userId))
            .Select(x => new TicketResponseDto(x.Id, x.Subject, x.IsOpen, x.CreatedAt))

            .AsQueryable();


        return Ok(ticketResponse);
        }

    }


    [HttpGet("{ticketId}")]
    public IActionResult GetDetail(Guid ticketId)
    {

        var messages = _applicationDbContext.TicketDetails
            .Where(o => o.TicketId == ticketId)
            .Include(p => p.AppUser)
            .OrderBy(c => c.CreatedAt)
            .ToList();

        Console.WriteLine(messages);


        return Ok(messages);
    }



    [HttpPost]
    public IActionResult SendMessage(TicketDetailRequestDto  requestDto)
    {

        TicketDetail ticketDetail = new();

        ticketDetail.TicketId = requestDto.TicketId;
        ticketDetail.Content = requestDto.Content;
        ticketDetail.CreatedAt = DateTime.Now;
        ticketDetail.AppUserId = requestDto.AppUserId;



        _applicationDbContext.Add(ticketDetail);
        _applicationDbContext.SaveChanges();



        return NoContent();
    }



    [HttpGet("{ticketId}")]
    public IActionResult GetByDetail(Guid ticketId)
    {

        var detail = _applicationDbContext.Tickets.
            Where(o => o.Id == ticketId)
            .Include(o => o.AppUser)
            .FirstOrDefault();   


        return Ok(detail);
    }


    [HttpGet("{ticketId}")]
    public IActionResult ChangeTicketStatus(Guid ticketId)
    {

        var existingTicket = _applicationDbContext.Tickets.Find(ticketId);
        if(existingTicket is not null)
        {
            existingTicket.IsOpen = false;
            _applicationDbContext.SaveChanges();
        }




        return NoContent();

    }




}
