using Backend.DTOs;

namespace Backend.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeGetDto>> GetAllAsync(int skip = 0, int take = 10);
        Task<List<EmployeeGetDto>> SearchAsync(string? searchTerm, DateTime? startDate, DateTime? endDate, int? departmentId = null, int skip = 0, int take = 10);
        Task<EmployeeGetDto> GetByIdAsync(int id);
        Task<EmployeeGetDto> CreateAsync(EmployeeCreateDto dto);
        Task<EmployeeGetDto> UpdateAsync(int id, EmployeeUpdateDto dto);
        Task DeleteAsync(int id);
        Task<int> GetCountAsync();
    }
}
