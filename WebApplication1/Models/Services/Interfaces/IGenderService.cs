namespace WebApplication1.Models.Services.Interfaces
{
    public interface IGenderService
    {
        public Task<List<Gender>> GetAllGenders();
    }
}
