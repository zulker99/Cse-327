using System.ComponentModel.DataAnnotations;

namespace AcademicAssistant.Models
{
    public class Book
    {
        [Key]
        public string ID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Course { get; set; }
        public string FileUrl { get; set; }

        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public string UserID { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
