namespace ITLANetworking.Core.Application.Dtos.Email
{
    public class EmailRequest
    {
        public string? To { get; set; }
        public required string Subject { get; set; }
        public required string Body { get; set; }
        public string From { get; set; } = string.Empty;
    }
}
