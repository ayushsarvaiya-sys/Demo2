using Backend.Data;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories.Implementations
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;

        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            // SARGABLE query - uses index on IsDeleted
            return await _context.Departments
                .AsNoTracking() 
                .Where(d => !d.IsDeleted)
                .OrderBy(d => d.Name)
                .ToListAsync();
        }
    }
}
