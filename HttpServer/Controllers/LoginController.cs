using HttpServer.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using HttpServer.Models.Requests;
using HttpServer.Models;
using Microsoft.Data.Sqlite;

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
        using var connection = InitDataBase();

        using var checkLoginCmd = connection.CreateCommand();
        checkLoginCmd.CommandText =
$"""
SELECT id, name FROM registred_users WHERE login = '{req.Login}' AND password = '{req.Password}'
""";
        using var checkResult = checkLoginCmd.ExecuteReader();
        if (!checkResult.Read())
        {
            return new LoginResult
            {
                Success = false,
                Description = $"Не удалось авторизоваться",
                Result = null,
            };
        }
        int id = checkResult.GetInt32(0);
        string name = checkResult.GetString(1);
        return new LoginResult
        {
            Success = true,
            Description = "Ok",
            Result = new User
            {
                Id = id,
                Name = name,
            }
        };
    }

    [HttpPost]
    [Route("register")]
    public RegisterResult Register(RegisterRequest req)
    {
        using var connection = InitDataBase();
        
        using var checkLoginCmd = connection.CreateCommand();
        checkLoginCmd.CommandText =
$"""
SELECT * FROM registred_users WHERE login = '{req.Login}'
""";
        using var checkResult = checkLoginCmd.ExecuteReader();
        if (checkResult.Read())
            return new RegisterResult
            {
                Success = false,
                Description = $"Пользователь с логином {req.Login} уже существует",
                Result = null,
            };

        using var insertCmd = connection.CreateCommand();
        insertCmd.CommandText =
$"""
INSERT INTO registred_users (name,login,password) 
VALUES ('{req.Name}','{req.Login}', '{req.Password}') 
RETURNING id
""";
        using var reader = insertCmd.ExecuteReader();
        reader.Read();
        int id = reader.GetInt32(0);

        return new RegisterResult
        {
            Success = true,
            Description = "Ok",
            Result = new User
            {
                Id = id,
                Name = req.Name,
            }
        };

    }

    private SqliteConnection InitDataBase()
    {
        string dirPlace = Environment.ProcessPath;
        string result = "";
        for (int i = dirPlace.Length - 1; i>=0; i--)
        {
            char c = dirPlace[i];
            if (c == '\\')
            {
                result = dirPlace.Substring(0, i);
                break;
            }
        }
        var connection = new SqliteConnection($"Data Source={result}\\data.db");
        connection.Open();
        using var table = connection.CreateCommand();
        table.CommandText =
"""
CREATE TABLE IF NOT EXISTS "registred_users" (
	"id"	INTEGER NOT NULL UNIQUE,
	"login"	TEXT NOT NULL UNIQUE,
	"password"	TEXT NOT NULL,
	"name"	TEXT,
	PRIMARY KEY("id" AUTOINCREMENT)
);
""";
        table.ExecuteNonQuery();
        return connection;
    }
}
