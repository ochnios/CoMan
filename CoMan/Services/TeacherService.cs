using CoMan.Models;
using CoMan.Data;

namespace CoMan.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TeacherService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TeacherUser> GetTeacherById(string id)
        {
            return await _unitOfWork.Teachers
                .GetByIdAsync(id);
        }

    }
}