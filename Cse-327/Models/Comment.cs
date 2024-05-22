using System.ComponentModel.DataAnnotations;

namespace AcademicAssistant.Models
{
    public class Comment
    {
        [Key]
        public string ID { get; set; }
        [Required]
        public string UserID { get; set; }
        [Required]
        public string PostID { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime DateTime { get; set; } = DateTime.Now;

    }
}
