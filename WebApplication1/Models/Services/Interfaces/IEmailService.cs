namespace WebApplication1.Models.Services.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string to, string subject, string body);
        public string GenerateCode();
    }
}
