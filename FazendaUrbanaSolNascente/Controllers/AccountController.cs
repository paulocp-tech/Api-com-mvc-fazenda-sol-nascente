using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Collections.Generic;

namespace FazendaUrbanaSolNascente.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        public IActionResult Authenticate(string username, string password, string redirectTo)
        {
            // Aqui você pode adicionar sua lógica para verificar o nome de usuário e senha
            if (username == "gerente" && password == "adm123") // Exemplo de verificação (ajuste conforme necessário)
            {
                // Cria os claims para autenticar o usuário
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "gerente") // Você pode adicionar outros roles aqui, se necessário
                };

                var identity = new ClaimsIdentity(claims, "login");
                var principal = new ClaimsPrincipal(identity);

                // Realiza a autenticação
                HttpContext.SignInAsync(principal);

                // Retorna a resposta com sucesso e o redirecionamento para o relatório desejado
                return Json(new { success = true, redirectTo = $"/Reports/{redirectTo}" });
            }
            else
            {
                // Se a autenticação falhar, retorna uma resposta de falha
                return Json(new { success = false });
            }
        }
    }
}
