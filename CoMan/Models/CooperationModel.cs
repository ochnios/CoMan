using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoMan.Models
{
    public enum CooperationStatus
    {
        Active,
        Ended,
        Archived
    }

    [Table("Cooperations")]
    public class CooperationModel : IDeletableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Start date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DisplayName("Status")]
        public CooperationStatus Status { get; set; }

        [DisplayName("End date")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Comment")]
        public string? Comment { get; set; }

        [RegularExpression(@"^[2345](\.5)?$", ErrorMessage = "The value must be a correct mark!")]
        [DisplayName("Mark")]
        public string? Mark { get; set; }

        [DisplayName("Topic")]
        public virtual TopicModel? Topic { get; set; }

        [ForeignKey("CooperationRequestId")]
        [DisplayName("Cooperation Request")]
        public virtual CooperationRequestModel? CooperationRequest { get; set; }

        [ForeignKey("StudentId")]
        [DisplayName("Student")]
        public virtual StudentUser? Student { get; set; }

        [DisplayName("Teacher")]
        public virtual TeacherUser? Teacher { get; set; }

        public virtual ICollection<CommentModel>? Comments { get; set; }

        public Boolean Deleted { get; set; }
    }
}
