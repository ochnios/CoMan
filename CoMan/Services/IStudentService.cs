using CoMan.Models;

namespace CoMan.Services
{
    public interface IStudentService
    {
        Task<StudentUser> GetStudentById(string id);
    }
}