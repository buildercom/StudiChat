namespace HttpServer.Models.Responses
{
    public class RegisterResult
    {
        public required bool Success { get; set; }
        public required string Description { get; set; }
        public required User? Result { get; set; }
    }
}
