namespace WebApplication1.Models.Services.Interfaces
{
    public interface IGenderService
    {
        public Task<List<Gender>> GetAllGenders();
        public Task<Gender?> GetGenderById(int id);
        public Task<Gender?> GetGenderByIdWithIncludeStudent(int id);
        public Task AddGender(Gender gender);
        public Task<string> UpdateGender(Gender gender);
        public Task<string> DeleteGender(int id);
        public Task<bool> IsGenderNameArExistForUpdate(string nameAr, int id);
        public Task<bool> IsGenderNameEnExistForUpdate(string nameEn, int id);
        public Task<bool> IsGenderNameEnExist(string nameEn);
        public Task<bool> IsGenderNameArExist(string nameAr);
    }
}
