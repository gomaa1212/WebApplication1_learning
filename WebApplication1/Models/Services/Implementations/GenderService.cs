using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Services.Interfaces;
using WebApplication1.Repository.Interfaces;
using WebApplication1.UnitOfWorks;

namespace WebApplication1.Models.Services.Implementations
{
    public class GenderService : IGenderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public GenderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddGender(Gender gender)
        {
           await _unitOfWork.Repository<Gender>().AddAsync(gender);
        }

        public async Task<string> DeleteGender(int id)
        {
            try
            {
                var gender = await _unitOfWork.Repository<Gender>().GetByIdAsync(id);
                if (gender != null)
                {
                    await _unitOfWork.Repository<Gender>().DeleteAsync(gender);
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
            var genders = await _unitOfWork.Repository<Gender>().GetAsQueryable().Include(x => x.Students).ToListAsync();
            return genders;
        }

        public async Task<Gender?> GetGenderById(int id)
        {
            return await _unitOfWork.Repository<Gender>().GetByIdAsync(id);
        }

        public async Task<Gender?> GetGenderByIdWithIncludeStudent(int id)
        {
            return await _unitOfWork.Repository<Gender>().GetAsQueryable().Include(g=>g.Students).ThenInclude(i=>i.StudentImages).FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<bool> IsGenderNameArExist(string nameAr)
        {
            return await _unitOfWork.Repository<Gender>().GetAsQueryable().AnyAsync(g => g.NameAr == nameAr);
        }

        public async Task<bool> IsGenderNameArExistForUpdate(string nameAr, int id)
        {
            return await _unitOfWork.Repository<Gender>().GetAsQueryable().AnyAsync(g => g.NameAr == nameAr && g.Id != id);
        }

        public async Task<bool> IsGenderNameEnExist(string nameEn)
        {
            return await _unitOfWork.Repository<Gender>().GetAsQueryable().AnyAsync(g => g.NameEn == nameEn);
        }

        public async Task<bool> IsGenderNameEnExistForUpdate(string nameEn, int id)
        {
            return await _unitOfWork.Repository<Gender>().GetAsQueryable().AnyAsync(g => g.NameEn == nameEn && g.Id != id);
        }
        public async Task<string> UpdateGender(Gender gender)
        {
            try
            {
                var existingGender = await _unitOfWork.Repository<Gender>().GetByIdAsync(gender.Id);
                if (existingGender != null)
                {
                    existingGender.NameEn = gender.NameEn;
                    existingGender.NameAr = gender.NameAr;
                    await _unitOfWork.Repository<Gender>().UpdateAsync(existingGender);
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
