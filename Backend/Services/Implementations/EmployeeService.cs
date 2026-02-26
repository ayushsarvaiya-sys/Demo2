using AutoMapper;
using Backend.DTOs;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;

namespace Backend.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<EmployeeGetDto>> GetAllAsync(int skip = 0, int take = 10)
        {
            var employees = await _repository.GetAllAsync(skip, take);
            return _mapper.Map<List<EmployeeGetDto>>(employees);
        }

        public async Task<EmployeeGetDto> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid employee ID");

            var employee = await _repository.GetByIdAsync(id);
            if (employee == null)
                throw new KeyNotFoundException("Employee not found");

            return _mapper.Map<EmployeeGetDto>(employee);
        }

        public async Task<EmployeeGetDto> CreateAsync(EmployeeCreateDto dto)
        {
            // Check if email already exists
            var existingEmployee = await _repository.GetByEmailAsync(dto.Email ?? string.Empty);
            if (existingEmployee != null)
                throw new InvalidOperationException("Employee email already exists");

            var employee = _mapper.Map<Employee>(dto);
            employee.IsDeleted = false;

            var created = await _repository.CreateAsync(employee);
            return _mapper.Map<EmployeeGetDto>(created);
        }

        public async Task<EmployeeGetDto> UpdateAsync(int id, EmployeeUpdateDto dto)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid employee ID");

            var employee = _mapper.Map<Employee>(dto);
            var updated = await _repository.UpdateAsync(id, employee);

            if (updated == null)
                throw new KeyNotFoundException("Employee not found or already deleted");

            return _mapper.Map<EmployeeGetDto>(updated);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid employee ID");

            var deleted = await _repository.DeleteAsync(id);
            if (!deleted)
                throw new KeyNotFoundException("Employee not found or already deleted");
        }

        public async Task<int> GetCountAsync()
        {
            return await _repository.CountAsync();
        }

        public async Task<List<EmployeeGetDto>> SearchAsync(string? searchTerm, DateTime? startDate, DateTime? endDate, int? departmentId = null, int skip = 0, int take = 10)
        {
            var employees = await _repository.SearchAsync(searchTerm, startDate, endDate, departmentId, skip, take);
            return _mapper.Map<List<EmployeeGetDto>>(employees);
        }
    }
}
