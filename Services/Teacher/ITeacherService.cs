using MyWebAPI.ViewModels.Teacher;

namespace MyWebAPI.Services.Teacher;

public interface ITeacherService
{
  Task<IEnumerable<TeacherViewVM>> GetAllAsync();

  Task<TeacherViewVM?> GetByIdAsync(int id);

  Task<TeacherViewVM> CreateAsync(TeacherCreateVM teacher);

  Task<bool> UpdateAsync(int id, TeacherUpdateVM teacher);

  Task<bool> DeleteAsync(int id);
}
