using CoMan.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoMan.Data
{
    public enum Role
    {
        Student,
        Teacher,
        Admin
    }

    public class ApplicationUser : IdentityUser
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

        public Boolean Deleted { get; set; }
    }
}
