using System.ComponentModel.DataAnnotations;

namespace AcademicAssistant.Models
{
    public class Test
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
