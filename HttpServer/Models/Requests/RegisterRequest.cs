namespace HttpServer.Models.Requests
{
    public class RegisterRequest
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
    }
}
