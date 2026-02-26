using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Designation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Designation Title is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Designation Title must be between 2 and 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Designation Code is required")]
        [StringLength(20, ErrorMessage = "Designation Code cannot exceed 20 characters")]
        public string Code { get; set; }

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string Description { get; set; }

        [Range(0, 999999.99, ErrorMessage = "Base Salary Range must be between 0 and 999999.99")]
        public decimal BaseSalaryRange { get; set; }

        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
