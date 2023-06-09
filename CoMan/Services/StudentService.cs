using CoMan.Models;
using CoMan.Data;

namespace CoMan.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<StudentUser> GetStudentById(string id)
        {
            return await _unitOfWork.Students
                .GetByIdAsync(id);
        }

    }
}