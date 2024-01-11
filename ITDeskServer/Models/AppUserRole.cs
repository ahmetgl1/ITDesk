using Microsoft.AspNetCore.Identity;

namespace ITDeskServer.Models;

public class AppUserRole 
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public AppRole? Role { get; set; }
    public Guid UserId { get; set; }

}
