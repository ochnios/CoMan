using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoMan.Models
{
    public enum TopicStatus
    {
        Active,
        Proposed,
        Archived
    }

    public class TopicModel
    {
        [Key]
        public int TopicId { get; set; }
        [DisplayName("Date added")]
        public DateTime AddedDate { get; set; }
        [DisplayName("Title")]
        public string Title { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Student limit")]
        public int StudentLimit { get; set; }
        [DisplayName("Status")]
        public TopicStatus Status { get; set; }
    }
}
