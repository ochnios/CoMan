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

        [DisplayName("Consideration date")]
        public DateTime? ConsiderationDate { get; set; }

        [Required]
        [DisplayName("Applicant")]
        // temporary string, should be a user - student
        public string Applicant { get; set; } = null!;

        [Required]
        [DisplayName("Recipent")]
        // temporary string, should be a user - teacher
        public string Recipent { get; set; } = null!;

        [Required]
        [DisplayName("Topic")]
        public TopicModel Topic { get; set; } = null!;

        [DisplayName("Applicant comment")]
        public string? ApplicantComment { get; set; }

        [DisplayName("Recipent comment")]
        public string? RecipentComment { get; set; }

        [Required]
        [DisplayName("Status")]
        public CooperationRequestStatus Status { get; set; }
    }
}
