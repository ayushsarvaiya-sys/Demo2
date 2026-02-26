using Backend.Models;

namespace Backend.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync(int skip = 0, int take = 10);
        Task<Employee> GetByIdAsync(int id);
        Task<Employee> GetByEmailAsync(string email);
        Task<IEnumerable<Employee>> SearchAsync(string? searchTerm, DateTime? startDate, DateTime? endDate, int? departmentId = null, int skip = 0, int take = 10);
        Task<Employee> CreateAsync(Employee employee);
        Task<Employee> UpdateAsync(int id, Employee employee);
        Task<bool> DeleteAsync(int id);
        Task<int> CountAsync();
    }
}
