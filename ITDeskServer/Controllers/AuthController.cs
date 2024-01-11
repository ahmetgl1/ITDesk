using Azure.Core;
using FluentValidation.Results;
using ITDeskServer.Abstraction;
using ITDeskServer.Context;
using ITDeskServer.DTOs.Requests;
using ITDeskServer.Models;
using ITDeskServer.Service;
using ITDeskServer.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITDeskServer.Controllers;

[AllowAnonymous]
public class AuthController : ApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly JWTService _jWTService;
   



    public AuthController(UserManager<AppUser> userManager,
                          ApplicationDbContext applicationDbContext,
                          SignInManager<AppUser> signInManager,
                          JWTService jWTService
                           )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jWTService = jWTService;
        _applicationDbContext = applicationDbContext;
       

    }

    [HttpPost]
   public async Task<IActionResult> Login(LoginDtoRequest request ,
                                    CancellationToken cancellationToken)
    {

        LoginValidate validate = new();
        ValidationResult validationResult=  validate.Validate(request);
    
        if (!validationResult.IsValid)
        {


            var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
            return StatusCode(422, errorMessages);

        }

        AppUser? appUser = await _userManager.FindByNameAsync(request.UserNameOrEmail);
        if (appUser is null)
        {
            appUser = await _userManager.FindByEmailAsync(request.UserNameOrEmail);
            if (appUser is null)
            {
                return BadRequest(new { Message = "Kullanıcı bulunamadı!" });
            }
        }




    var result = await _signInManager.CheckPasswordSignInAsync(appUser, request.Password, true);


        if (result.IsLockedOut && appUser.LockoutEnd is not null)
        {



        TimeSpan? timeSpan =  (appUser.LockoutEnd) - DateTime.UtcNow;

            return BadRequest(new { Message = "Şifreniz " + Math.Ceiling(timeSpan.Value.TotalMinutes) + " e kadar kilitlendi" });
        }


        if (result.IsNotAllowed)
        {

            return BadRequest(new { Message = "Emailiniz Onaylı Değil" });
        }
        if (!result.Succeeded)
        {

            return BadRequest(new { Message = "Şifreniz Hatalı" });
        }



      var roles = _applicationDbContext.AppUserRoles
            .Where(o => o.UserId == appUser.Id)
            .Include(p => p.Role)
            .Select(s => s.Role!.Name)
            .ToList(); 

      var resultToken = _jWTService.CreateToken(appUser ,roles, request.RememberMe);


        return Ok(new {AccessToken = resultToken});
    }



    [HttpPost]
    public async Task<IActionResult> GoogleLogin(GoogleLoginDtoRequest googleLoginDtoRequest , 
                                                 CancellationToken cancellationToken)
    {

        AppUser? appUser = await _userManager.FindByEmailAsync(googleLoginDtoRequest.Email);



        if(appUser is not null)
        {

            string token = _jWTService.CreateToken(appUser, new(),true);
            return Ok(new {AccessToken = token});
        }

        string userName = googleLoginDtoRequest.Email;

        appUser = new()
        {
            Email = googleLoginDtoRequest.Email,
            FirstName = googleLoginDtoRequest.FirstName,
            LastName = googleLoginDtoRequest.LastName,
            UserName = userName,
            GoogleProvideId = googleLoginDtoRequest.Id
        };


        IdentityResult result = await _userManager.CreateAsync(appUser);



        if (result.Succeeded)
        {
            string token = _jWTService.CreateToken(appUser,new(), true);
            return Ok(new { AccessToken = token });
        }


        IdentityError? errorResult = result.Errors.FirstOrDefault();

        string errorMessage = errorResult is null ? "Giriş Yapma Esnasında Bir Hata Oluştu Lütfen Yetkili ile İletişime Geçin"
            : errorResult.Description;




        return BadRequest(new {Message = errorMessage});
    }




    








}
