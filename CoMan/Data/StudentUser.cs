using CoMan.Models;

namespace CoMan.Data
{
    public class StudentUser : ApplicationUser
    {
        public CooperationRequestModel? CooperationRequest { get; set; }

        public CooperationModel? Cooperation { get; set; }
    }
}
