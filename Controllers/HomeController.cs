using Microsoft.AspNetCore.Mvc;
using Project.MvcClient.Models;
using Project.MvcClient.Models.DTO.User;
using Project.MvcClient.Services.Api;
using System.Diagnostics;

namespace Project.MvcClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiClient _apiClient;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApiClient apiClient, ILogger<HomeController> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var conectou = await _apiClient.TestConnectionAsync();
            ViewBag.Conectada = conectou;

            if (conectou)
            {
                var users = await _apiClient.GetUsersAsync();
                return View(users);
            }

            return View(new List<UserDetailsDto>());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
