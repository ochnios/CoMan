using CoMan.Models;

namespace CoMan.Data
{
    public class StudentUser : ApplicationUser
    {
        public virtual CooperationRequestModel? CooperationRequest { get; set; }

        public virtual CooperationModel? Cooperation { get; set; }
    }
}
