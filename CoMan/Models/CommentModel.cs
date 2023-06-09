using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoMan.Models
{
    [Table("Comments")]
    public class CommentModel : IDeletableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Added date")]
        public DateTime AddedDate { get; set; }

        [Required]
        [DisplayName("Comment")]
        public string? Comment { get; set; }

        [Required]
        [DisplayName("Cooperation")]
        public virtual CooperationModel? Cooperation { get; set; }

        [Required]
        [DisplayName("Author")]
        public virtual ApplicationUser? Author { get; set; }

        public bool Deleted { get; set; }
    }
}
