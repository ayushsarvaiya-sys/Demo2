using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class DepartmentGetDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
    }
}
