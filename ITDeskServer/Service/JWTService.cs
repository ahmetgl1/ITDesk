using ITDeskServer.Models;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ITDeskServer.Service;

public  class JWTService
{

    public string  CreateToken( AppUser appUser ,List<string?>? roles,  bool RememberMe = false)
    {

        string token = string.Empty;

        DateTime expire = RememberMe == true ? DateTime.Now.AddMonths(1) : DateTime.Now.AddDays(1);


        List<Claim> claims = new List<Claim>();
                claims.Add(new ("UserId" , appUser.Id.ToString()));

              if(appUser.UserName is not null)
                    claims.Add(new ("UserName" , appUser.UserName.ToString()));
        
                     claims.Add(new ("Name" , appUser.GetName() ?? string.Empty));


        if(roles is not null)
        {

                     claims.Add(new("Roles", string.Join(",", roles)));
        }

                    if(appUser.Email is not null)
                              claims.Add(new("Email", appUser.Email)); 
         

        JwtSecurityToken securityToken = new(
            issuer: "Ahmet Güzeller",
            audience: "IT Desk",
            notBefore: DateTime.UtcNow,
            expires: expire,
             claims: claims,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Doldur be Meyhaneci dsfsfdsaf"))          
                                  , SecurityAlgorithms.HmacSha256)

            );


        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        token = handler.WriteToken(securityToken);




        return token;
    }

}
