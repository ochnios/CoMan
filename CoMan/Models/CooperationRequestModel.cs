using CoMan.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoMan.Models
{
    public enum CooperationRequestStatus
    {
        Waiting,
        Accepted,
        Rejected
    }

    [Table("CooperationRequests")]
    public class CooperationRequestModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Creation date")]
        public DateTime CreationDate { get; set; }

        [Required]
        [DisplayName("Status")]
        public CooperationRequestStatus Status { get; set; }

        [DisplayName("Consideration date")]
        public DateTime? ConsiderationDate { get; set; }

        [DisplayName("Student comment")]
        public string? ApplicantComment { get; set; }

        [DisplayName("Teacher comment")]
        public string? RecipentComment { get; set; }

        [DisplayName("Topic")]
        public virtual TopicModel Topic { get; set; } = null!;

        [ForeignKey("StudentId")]
        [DisplayName("Student")]
        public virtual StudentUser? Student { get; set; }

        [DisplayName("Teacher")]
        public virtual TeacherUser? Teacher { get; set; }

        [DisplayName("Cooperation")]
        public virtual CooperationModel? Cooperation { get; set; }
    }
}
