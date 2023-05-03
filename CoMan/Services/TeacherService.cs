using System.Collections.Generic;
using System.Threading.Tasks;
using CoMan.Models;
using CoMan.Data;
using Microsoft.AspNetCore.Identity;

namespace CoMan.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public TeacherService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<TeacherUser> GetTeacherById(string id)
        {
            return await _unitOfWork.Teachers
                .GetByIdAsync(id);
        }

    }
}