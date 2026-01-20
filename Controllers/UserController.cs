using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.MvcClient.Models.DTO.User;
using Project.MvcClient.Services.Api;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Project.MvcClient.Controllers
{
    public class UserController : Controller
    {
        private readonly ApiClient _apiClient;

        public UserController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        // VIEWS
        public async Task<IActionResult> Index()
        {
            var conectou = await _apiClient.TestConnectionAsync();
            ViewBag.Conectada = conectou;

            if (conectou)
            {
                var users = await _apiClient.GetUsersAsync();
                users = users?.OrderBy(u => u.Id).ToList();

                return View(users);
            }

            return View(new List<UserDetailsDto>());
        }

        public IActionResult NewUser()
        {
            return View(new UserCreateDto());
        }

        // Carrega os campos da view com os dados do usuário
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _apiClient.GetUserById(id);  // UserDetailsDto
            if (user == null) return NotFound();

            // CRÍTICO: converte pra UserEditDto
            var editDto = new UserEditDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                Email = user.Email
            };

            ViewData["Title"] = $"Editar - {user.FullName}";
            return View(editDto);  // ← UserEditDto!
        }

        // Operações do CRUD

        [HttpPost]
        [ActionName("NewUser")]
        public async Task<IActionResult> AddNewUser(UserCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(dto);


                var user = new UserCreateDto
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Age = dto.Age,
                    Email = dto.Email
                };

                var result = await _apiClient.CreateUserAsync(dto);
                TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Não foi possível cadastrar o usuário!\nErro: {ex.Message}";

                return RedirectToAction("Index");
            }            
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(int id, UserEditDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(dto);

                var userEdited = new UserEditDto
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Age = dto.Age,
                    Email = dto.Email
                };

                var result = await _apiClient.UpdateUserAsync(id, userEdited);
                TempData["MensagemSucesso"] = "Usuário editado com sucesso!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Não foi possível editar o usuário!\nErro: {ex.Message}";

                return RedirectToAction("Index");
            }
        }
    }
}
