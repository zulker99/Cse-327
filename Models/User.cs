using System.ComponentModel.DataAnnotations;

namespace AcademicAssistant.Models
{
    public class User
    {
        [Key]
        public string ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime DateTime { get; set; } = DateTime.Now;
        [Required]
        public string AccountType { get; set; }
    }
}
