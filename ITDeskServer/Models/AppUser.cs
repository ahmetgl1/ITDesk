using Microsoft.AspNetCore.Identity;

namespace ITDeskServer.Models;

public sealed class AppUser : IdentityUser<Guid>
{

    public string? FirstName { get; set; } = String.Empty;
    public string? LastName { get; set; } = String.Empty;
   

    public string? GoogleProvideId { get; set; }

    public string GetName()
    {

        return   (FirstName  + " " +  LastName);   

    }






}
