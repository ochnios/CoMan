using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CoMan.Models
{
    public enum Role
    {
        Student,
        Teacher,
        Admin
    }

    public class ApplicationUser : IdentityUser, IDeletableEntity
    {
        [Required]
        [MaxLength(256)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(256)]
        public string LastName { get; set; } = null!;

        public virtual ICollection<TopicModel>? Topics { get; set; }

        public virtual ICollection<CooperationRequestModel>? CooperationsRequests { get; set; }

        public virtual ICollection<CooperationModel>? Cooperations { get; set; }

        public bool Deleted { get; set; }
    }
}
