using CoMan.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace CoMan.Data
{
    public class DataSeeder
    {
        private readonly ILogger _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public DataSeeder(ILogger logger, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
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

        public async Task SeedTopicsFromJSON(string jsonPath)
        {
            string json = File.ReadAllText(jsonPath);
            List<TopicModel> topics = JsonConvert.DeserializeObject<List<TopicModel>>(json);

            foreach (var topic in topics)
            {
                _logger.LogInformation($"Adding topic: {topic.Id}");
                // _unitOfWork.Topics.AddAsync(topic);
            }
        }
    }
}
