using System.ComponentModel.DataAnnotations;

namespace AcademicAssistant.Models
{
    public class _ShowPost
    {
        [Key]
        public string ID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string ImgUrl { get; set; }

        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public string UserID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
