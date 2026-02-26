using Backend.Data;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync(int skip = 0, int take = 10)
        {
            // SARGABLE query - uses index on IsDeleted
            return await _context.Employees
                .AsNoTracking()
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .Where(e => !e.IsDeleted)
                .OrderBy(e => e.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            // SARGABLE query - filtered by IsDeleted
            return await _context.Employees
                .AsNoTracking()
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .Where(e => e.Id == id && !e.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<Employee> GetByEmailAsync(string email)
        {
            // SARGABLE query - uses index on Email and IsDeleted
            return await _context.Employees
                .AsNoTracking()
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .Where(e => e.Email == email && !e.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<Employee> CreateAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateAsync(int id, Employee employee)
        {
            var existingEmployee = await _context.Employees.FindAsync(id);
            if (existingEmployee == null || existingEmployee.IsDeleted)
                return null;

            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.Email = employee.Email;
            existingEmployee.DepartmentId = employee.DepartmentId;
            existingEmployee.DesignationId = employee.DesignationId;

            _context.Employees.Update(existingEmployee);
            await _context.SaveChangesAsync();
            return existingEmployee;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // Soft delete
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null || employee.IsDeleted)
                return false;

            employee.IsDeleted = true;
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> CountAsync()
        {
            // SARGABLE query - uses index on IsDeleted
            return await _context.Employees
                .Where(e => !e.IsDeleted)
                .CountAsync();
        }

        public async Task<IEnumerable<Employee>> SearchAsync(string? searchTerm, DateTime? startDate, DateTime? endDate, int? departmentId = null, int skip = 0, int take = 10)
        {
            // Build query with multiple filter conditions
            var query = _context.Employees
                .AsNoTracking()
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .Where(e => !e.IsDeleted);

            // Apply search term filter - searches across multiple fields
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim().ToLower();
                query = query.Where(e =>
                    e.Id.ToString().Contains(searchTerm) ||
                    e.FirstName.ToLower().Contains(searchTerm) ||
                    e.LastName.ToLower().Contains(searchTerm) ||
                    (e.FirstName + " " + e.LastName).ToLower().Contains(searchTerm) ||
                    e.Email.ToLower().Contains(searchTerm) ||
                    e.Department.Name.ToLower().Contains(searchTerm) ||
                    e.Designation.Title.ToLower().Contains(searchTerm)
                );
            }

            // Apply department filter
            if (departmentId.HasValue && departmentId.Value > 0)
            {
                query = query.Where(e => e.DepartmentId == departmentId.Value);
            }

            // Apply date range filter
            if (startDate.HasValue)
            {
                query = query.Where(e => e.CreatedAt >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                // Add one day to endDate to include the entire end date
                var endDatePlusOne = endDate.Value.AddDays(1);
                query = query.Where(e => e.CreatedAt < endDatePlusOne);
            }

            return await query
                .OrderByDescending(e => e.CreatedAt)
                .ThenBy(e => e.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
    }
}
