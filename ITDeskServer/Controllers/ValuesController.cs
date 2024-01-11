using ITDeskServer.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITDeskServer.Controllers;

[Authorize(AuthenticationSchemes ="Bearer")]
public class ValuesController : ApiController
{

    [HttpGet]
    public IActionResult Get()
    {


        string result = "Api Çalıştı";
        return Ok(result);
    }


}
