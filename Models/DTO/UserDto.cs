using System.ComponentModel.DataAnnotations;

namespace ApiDevBP.Models.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The last name is mandatory")]
        public string Lastname { get; set; }
    }
}
