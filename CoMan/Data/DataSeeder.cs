using CoMan.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace CoMan.Data
{
    public class DataSeeder
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeeder(ILogger logger, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedRoles(string[] roles)
        {
            foreach (string role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public async Task SeedUsersFromJSON(string jsonPath)
        {
            if (_userManager.Users.Any())
            {
                return;
            }

            string json = File.ReadAllText(jsonPath);
            List<UserData> users = JsonConvert.DeserializeObject<List<UserData>>(json)!;

            foreach (var user in users)
            {
                _logger.LogInformation($"Adding user: {user.Email}");

                ApplicationUser created = CreateUser(user.Role);
                created.FirstName = user.FirstName;
                created.LastName = user.LastName;
                created.UserName = user.Email;
                created.Email = user.Email;

                var result = await _userManager.CreateAsync(created, user.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(created, user.Role);
                }
            }
        }

        public async Task SeedTopicsFromJSON(string jsonPath)
        {
            if ((await _unitOfWork.Topics.GetAllAsync()).Any())
            {
                return;
            }

            string json = File.ReadAllText(jsonPath);
            List<TopicModel> topics = JsonConvert.DeserializeObject<List<TopicModel>>(json)!;

            Random random = new Random();
            List<TeacherUser> teachers = _userManager.Users.OfType<TeacherUser>().ToList();

            foreach (var topic in topics)
            {
                // _logger.LogInformation($"Adding topic: {topic.Id}");

                int teacherIndex = random.Next(teachers.Count);
                topic.Author = teachers[teacherIndex];
                await _unitOfWork.Topics.AddAsync(topic);
            }

            await _unitOfWork.CommitAsync();
        }

        private ApplicationUser CreateUser(string role)
        {
            switch (role)
            {
                case "Admin":
                    {
                        return Activator.CreateInstance<AdminUser>();
                    }
                case "Teacher":
                    {
                        return Activator.CreateInstance<TeacherUser>();
                    }
                case "Student":
                    {
                        return Activator.CreateInstance<StudentUser>();
                    }
                default:
                    {
                        throw new Exception($"Unknown role: {role}");
                    }
            }
        }
    }

    public class UserData
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Role { get; set; } = null!;
    }
}
