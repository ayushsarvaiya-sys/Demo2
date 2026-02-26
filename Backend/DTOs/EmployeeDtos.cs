using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class EmployeeCreateDto
    {
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 50 characters")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 50 characters")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email format is invalid")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Department ID is required")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Designation ID is required")]
        public int DesignationId { get; set; }
    }

    public class EmployeeUpdateDto
    {
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 50 characters")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 50 characters")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email format is invalid")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Department ID is required")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Designation ID is required")]
        public int DesignationId { get; set; }
    }

    public class EmployeeGetDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int DesignationId { get; set; }
        public string? DesignationTitle { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
