using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models.Dto;

public class LoginResponce
{
    public required bool Success { get; set; }
    public required string Description { get; set; }
    public UserDto Result { get; set; }
}
