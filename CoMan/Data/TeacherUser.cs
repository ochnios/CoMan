using CoMan.Models;

namespace CoMan.Data
{
    public class TeacherUser : ApplicationUser
    {
        public virtual ICollection<TopicModel>? Topics { get; set; }

        public virtual ICollection<CooperationRequestModel>? CooperationsRequests { get; set; }

        public virtual ICollection<CooperationModel>? Cooperations { get; set; }

        public int MaxCooperations { get; set; }
    }
}
