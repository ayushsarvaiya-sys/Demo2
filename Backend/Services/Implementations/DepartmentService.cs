using AutoMapper;
using Backend.DTOs;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;

namespace Backend.Services.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<DepartmentGetDto>> GetAllAsync()
        {
            var departments = await _repository.GetAllAsync();
            return _mapper.Map<List<DepartmentGetDto>>(departments);
        }
    }
}
