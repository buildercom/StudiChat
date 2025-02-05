using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models;

public class Frame
{
    public int AuthorId { get; set; }
    public string AuthorName { get; set; }
    public int ReceiverId { get; set; }
    public bool IsHeartbeat { get; set; }
    public string Message { get; set; }
}
