using HttpServer.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using HttpServer.Models.Requests;
using HttpServer.Models;

namespace HttpServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    [HttpGet]
    public string CheckLogin()
    {
        return "Hello world.";
    }

    [HttpPost]
    public LoginResult Login(LoginRequest req)
    {
        //var loginResult = new LoginResult();
        if (req.Login == "boris" && req.Password == "kaban")
        {
            return new LoginResult
            {
                Success = true,
                Description = "Ok",
                Result = new User
                {
                    Id = 338,
                    Name = "Boris",
                }
                
            };
        }
        else
        {
            return new LoginResult
            {
                Success = false,
                Description = "Данного пользователя не существует.",
                Result = null,
            };
        }

        return new LoginResult
        {
            Success = true,
            Description = "Ok",
            Result = new User
            {
                Id = 1,
                Name = "AdamEva"
            }
        };

    }

    [HttpPost]
    [Route("register")]
    public  RegisterResult Register (RegisterRequest req)
    {
        return new RegisterResult
        {
            Success = true,
            Description = "Ok",
            Result = new User
            {
                Id = 1,
                Name = "AdamEva"
            }
        };

    }

}
