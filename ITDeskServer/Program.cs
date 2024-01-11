
using ITDeskServer.Context;
using ITDeskServer.Middleware;
using ITDeskServer.Models;
using ITDeskServer.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace ITDeskServer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.



            builder.Services.AddCors(configure =>
            {
                configure.AddDefaultPolicy(o =>
                {
                    o.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                });
            });



            builder.Services.AddAuthentication().AddJwtBearer(configuration =>
            {

                configuration.TokenValidationParameters = new()
                {
                    ValidateIssuer = true, //token gönderen kiþi
                    ValidateAudience = true, //token kullanacak bilgisi
                    ValidateIssuerSigningKey = true, //token üretmeyi saðlayan güvenlik sözcüðü 
                    ValidateLifetime = true,


                    ValidIssuer = "Ahmet Güzeller",
                    ValidAudience = "IT Desk",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Doldur be Meyhaneci dsfsfdsaf"))
         };
            });

            builder.Services.AddDbContext<ApplicationDbContext>(o =>
            {
                
                o.UseSqlServer("Data Source=DESKTOP-3O4V1S5;Initial Catalog=ITDeskDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            });

            builder.Services.AddIdentity<AppUser , AppRole>(o =>
            {
                o.Password.RequiredLength = 6;
                o.SignIn.RequireConfirmedEmail = true;
                o.Lockout.MaxFailedAccessAttempts = 3;
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                o.Lockout.AllowedForNewUsers = true;

            }).AddEntityFrameworkStores<ApplicationDbContext>();



            builder.Services.AddScoped<JWTService>();




            builder.Services.AddControllers().AddOData(options => options.EnableQueryFeatures());

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


            builder.Services.AddSwaggerGen(setup =>
            {
                var jwtSecuritySheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Put **_ONLY_** yourt JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.AddSecurityDefinition(jwtSecuritySheme.Reference.Id, jwtSecuritySheme);

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecuritySheme, Array.Empty<string>() }
                });
            });




            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

        //    app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();



            using (var scoped = app.Services.CreateScope())
            {
                var context =  scoped.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                context.Database.Migrate();

            }

            await ExtensionsMiddleware.CreateFirstUserAsync(app);
             ExtensionsMiddleware.CreateRoles(app);
             ExtensionsMiddleware.CreateUserRole(app);
            ExtensionsMiddleware.AutoMigration(app);

            app.UseCors();

            app.Run();
        }
    }
}