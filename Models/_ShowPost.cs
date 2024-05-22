using System.ComponentModel.DataAnnotations;

namespace AcademicAssistant.Models
{
    public class CommentViewModel
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public string PostID { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public DateTime DateTime { get; set; }
        public string UserName { get; set; }  // New property for UserName
    }


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
        public List<CommentViewModel> Comments { get; set; }
    }
}
