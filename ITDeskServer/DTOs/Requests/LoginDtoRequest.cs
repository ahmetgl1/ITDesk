namespace ITDeskServer.DTOs.Requests;

public sealed record LoginDtoRequest
(
    string UserNameOrEmail,
     string Password,
     bool RememberMe  = false);


