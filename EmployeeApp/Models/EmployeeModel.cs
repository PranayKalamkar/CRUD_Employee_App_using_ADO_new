using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeApp.Models
{
    [Table("employee")]
    public class EmployeeModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string? firstName { get; set; }

        [Required]
        [StringLength(30)]
        public string? lastName { get; set; }

        [Required]
        [StringLength(10)]
        public string? contactNumber { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string? emailId { get; set; }

        [Required]
        [StringLength(3)]
        public string? age { get; set; }

        [Display(Name = "Image Path")]
        public string? imagePath { get; set; }

        [Display(Name = "Image File")]
        [Required(ErrorMessage = "Please choose a file")]
        public IFormFile imageFile { get; set; }
    }
}
