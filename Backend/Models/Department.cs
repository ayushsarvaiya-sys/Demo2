using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Department Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Department Name must be between 2 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Department Code is required")]
        [StringLength(20, ErrorMessage = "Department Code cannot exceed 20 characters")]
        public string Code { get; set; }

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string Description { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<Designation> Designations { get; set; } = new List<Designation>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
