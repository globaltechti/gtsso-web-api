using Globaltech.GtSso.WebApiSample;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Globatech.GtSso.WebApiSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = Policies.PolicyBasedOnScope)]
    public class ProfileController : ControllerBase
    {
        private static readonly string[] Profiles = new[]
        {
           "Sales", "Marketing", "Human Resources", "Finance", "Customer Support", "Engineering", "Product Development", "Research and Development (R&D)", "Operations", "Legal", "IT (Information Technology)", "Quality Assurance (QA)", "Administration", "Business Development", "Supply Chain", "Logistics", "Training and Development", "Public Relations (PR)", "Accounting", "Design"
        };

        private readonly ILogger<ProfileController> _logger;

        public ProfileController(ILogger<ProfileController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "profile")]
        public IEnumerable<Department> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Department
            {
                Date = DateTime.Now.AddDays(index),
                Name = Profiles[Random.Shared.Next(Profiles.Length)]
            })
            .ToArray();
        }
    }
}
