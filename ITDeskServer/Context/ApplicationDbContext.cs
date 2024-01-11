using ITDeskServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ITDeskServer.Context;

public class ApplicationDbContext : IdentityDbContext<AppUser , AppRole , Guid>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }


public DbSet<Ticket> Tickets { get; set; }
public DbSet<TicketDetail> TicketDetails { get; set; }
public DbSet<TicketFile> TicketFiles { get; set; }
public DbSet<AppUserRole> AppUserRoles { get; set; }




    protected override void OnModelCreating(ModelBuilder builder)
    {

        //builder.Entity<IdentityUserLogin<Guid>>().HasKey(o => o.UserId);
        //builder.Entity<IdentityUserToken<Guid>>().HasKey(o => o.UserId);
        //builder.Entity<IdentityUserClaim<Guid>>().HasKey(o => o.Id);


        builder.Ignore<IdentityUserLogin<Guid>>();
        builder.Ignore<IdentityUserToken<Guid>>();
        builder.Ignore<IdentityUserClaim<Guid>>();
        builder.Ignore<IdentityRoleClaim<Guid>>();

        builder.Entity<IdentityUserRole<Guid>>().HasKey(p => new { p.RoleId, p.UserId });

        builder.Entity<AppUserRole>().HasKey(x => x.Id);


    }


}
