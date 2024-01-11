using FluentValidation;
using ITDeskServer.DTOs.Requests;

namespace ITDeskServer.Validators;

public class LoginValidate : AbstractValidator<LoginDtoRequest>
{
    public LoginValidate()
    {

       
        RuleFor(o => o.UserNameOrEmail).NotEmpty().WithMessage("Kullanıcı adı veya email adresi boş olamaz");
        
        RuleFor(o => o.UserNameOrEmail).MinimumLength(6).WithMessage("Kullanıcı adı veya email adresi 6 haneden uzun olmalıdır");

        RuleFor(p => p.Password).NotEmpty().WithMessage("Geçerli şifre giriniz");

        RuleFor(p => p.Password).Matches("[A-Z]").WithMessage("Şifre  an az 1 büyük harf içerlemlidir");
        
        RuleFor(p => p.Password).Matches("[a-z]").WithMessage("Şifre  an az 1 küçük harf içerlemlidir");

        RuleFor(p => p.Password).Matches("[0-9]").WithMessage("Şifre  an az 1 rakam içerlemlidir");

        RuleFor(p => p.Password).Matches("[^a-zA-z0-9]").WithMessage("Şifre  an az 1 özel karakter içerlemlidir");





    }
}
