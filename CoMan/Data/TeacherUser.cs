using CoMan.Models;

namespace CoMan.Data
{
    public class TeacherUser : ApplicationUser
    {
        public ICollection<TopicModel>? Topics { get; set; }

        public ICollection<CooperationRequestModel>? CooperationsRequests { get; set; }

        public ICollection<CooperationModel>? Cooperations { get; set; }

        public int MaxCooperations { get; set; }
    }
}
