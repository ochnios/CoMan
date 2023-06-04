using CoMan.Models;

namespace CoMan.Services
{
    public interface ITeacherService
    {
        Task<TeacherUser> GetTeacherById(string id);
    }
}