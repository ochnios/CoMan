using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoMan.Models
{
    public enum TopicStatus
    {
        Active,
        Proposed,
        Archived
    }

    [Table("Topics")]
    public class TopicModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Date added")]
        public DateTime AddedDate { get; set; }

        [Required]
        [DisplayName("Status")]
        public TopicStatus Status { get; set; }

        [Required]
        [DisplayName("Title")]
        public string Title { get; set; } = null!;

        [Required]
        [DisplayName("Student limit")]
        public int StudentLimit { get; set; }

        [DisplayName("Description")]
        public string? Description { get; set; }

        [DisplayName("Author")]
        // temporary string, should be a user - teacher or student
        public string Author { get; set; } = null!;
    }
}
