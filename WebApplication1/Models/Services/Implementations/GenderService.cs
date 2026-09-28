using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Services.Interfaces;
using WebApplication1.Repository.Interfaces;

namespace WebApplication1.Models.Services.Implementations
{
    public class GenderService : IGenderService
    {
        private readonly IGenderRepository _GenderRepository;
        public GenderService(IGenderRepository genderRepository)
        {
            _GenderRepository = genderRepository;
        }

        public async Task AddGender(Gender gender)
        {
           await _GenderRepository.AddAsync(gender);
        }

        public async Task<string> DeleteGender(int id)
        {
            try
            {
                var gender = await _GenderRepository.GetByIdAsync(id);
                if (gender != null)
                {
                    await _GenderRepository.DeleteAsync(gender);
                    return "Gender deleted successfully.";
                }
                else
                {
                    return "Gender not found.";
                }
            }
            catch (Exception ex)
            {
                return "An error occurred while deleting the gender.";
            }
        }

        public async Task<List<Gender>> GetAllGenders()
        {
            var genders = await _GenderRepository.GetAsQueryable().Include(x => x.Students).ToListAsync();
            return genders;
        }

        public async Task<Gender?> GetGenderById(int id)
        {
            return await _GenderRepository.GetByIdAsync(id);
        }

        public async Task<Gender?> GetGenderByIdWithIncludeStudent(int id)
        {
            return await _GenderRepository.GetAsQueryable().Include(g=>g.Students).ThenInclude(i=>i.StudentImages).FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<bool> IsGenderNameArExist(string nameAr)
        {
            return await _GenderRepository.GetAsQueryable().AnyAsync(g => g.NameAr == nameAr);
        }

        public async Task<bool> IsGenderNameArExistForUpdate(string nameAr, int id)
        {
            return await _GenderRepository.GetAsQueryable().AnyAsync(g => g.NameAr == nameAr && g.Id != id);
        }

        public async Task<bool> IsGenderNameEnExist(string nameEn)
        {
            return await _GenderRepository.GetAsQueryable().AnyAsync(g => g.NameEn == nameEn);
        }

        public async Task<bool> IsGenderNameEnExistForUpdate(string nameEn, int id)
        {
            return await _GenderRepository.GetAsQueryable().AnyAsync(g => g.NameEn == nameEn && g.Id != id);
        }
        public async Task<string> UpdateGender(Gender gender)
        {
            try
            {
                var existingGender = await _GenderRepository.GetByIdAsync(gender.Id);
                if (existingGender != null)
                {
                    existingGender.NameEn = gender.NameEn;
                    existingGender.NameAr = gender.NameAr;
                    await _GenderRepository.UpdateAsync(existingGender);
                    return "Gender updated successfully.";
                }
                else
                {
                    return "Gender not found.";
                }

            }
            catch (Exception ex)
            {
                return "An error occurred while updating the gender.";
            }
        }
    }     
}
