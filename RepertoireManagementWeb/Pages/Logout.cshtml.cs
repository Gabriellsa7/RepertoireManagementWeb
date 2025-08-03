using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;

namespace RepertoireManagementWeb.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            // Remove o cookie de autentica��o
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Limpa sess�o se voc� usa
            HttpContext.Session.Clear();

            // Redireciona para a p�gina de login (ou onde quiser)
            return RedirectToPage("/Login");
        }
    }
}
