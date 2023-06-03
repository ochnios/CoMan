using CoMan.Data;
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
    public class TopicModel : IDeletableEntity
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
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        [DisplayName("Student limit")]
        public int StudentLimit { get; set; }

        [DisplayName("Description")]
        public string? Description { get; set; }

        [Required]
        [DisplayName("Author")]
        public virtual TeacherUser Author { get; set; } = null!;

        public virtual ICollection<CooperationRequestModel>? CooperationRequests { get; set; }

        public virtual ICollection<CooperationModel>? Cooperations { get; set; }

        public Boolean Deleted { get; set; }
    }
}
