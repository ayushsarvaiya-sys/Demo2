using Backend.DTOs;

namespace Backend.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<DepartmentGetDto>> GetAllAsync();
    }
}
