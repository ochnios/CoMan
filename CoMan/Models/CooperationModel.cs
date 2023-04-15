using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoMan.Models
{
    public enum CooperationStatus
    {
        Active,
        Ended
    }

    [Table("Cooperations")]
    public class CooperationModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Start date")]
        public DateTime CreationDate { get; set; }

        [Required]
        [DisplayName("Status")]
        public CooperationStatus Status { get; set; }

        [DisplayName("End date")]
        public DateTime? ConsiderationDate { get; set; }

        [DisplayName("Comment")]
        public string? Comment { get; set; }

        [DisplayName("Mark")]
        public float? Mark { get; set; }

        [DisplayName("Topic")]
        public TopicModel Topic { get; set; } = null!;

        [DisplayName("Cooperation Request")]
        public CooperationRequestModel CooperationRequest { get; set; } = null!;

        [DisplayName("Student")]
        // temporary string, should be a user - student
        public string Student { get; set; } = null!;

        [DisplayName("Teacher")]
        // temporary string, should be a user - teacher
        public string Teacher { get; set; } = null!;
    }
}
