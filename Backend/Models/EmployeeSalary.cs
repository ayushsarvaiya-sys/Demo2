using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class EmployeeSalary
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [Required(ErrorMessage = "Base Salary is required")]
        [Range(0, 999999.99, ErrorMessage = "Base Salary must be between 0 and 999999.99")]
        public decimal BaseSalary { get; set; }

        [Range(0, 999999.99, ErrorMessage = "HRA must be between 0 and 999999.99")]
        public decimal HRA { get; set; }

        [Range(0, 999999.99, ErrorMessage = "DA must be between 0 and 999999.99")]
        public decimal DA { get; set; }

        [Range(0, 999999.99, ErrorMessage = "Allowances must be between 0 and 999999.99")]
        public decimal Allowances { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
