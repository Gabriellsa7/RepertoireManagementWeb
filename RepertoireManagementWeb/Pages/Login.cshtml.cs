using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RepertoireManagementWeb.Data;
using RepertoireManagementWeb.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RepertoireManagementWeb.Pages;

public class LoginModel : PageModel
{
    private readonly AppDbContext _context;

    public LoginModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public string LoginEmail { get; set; }

    [BindProperty]
    public string LoginPassword { get; set; }

    public string ErrorMessage { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == LoginEmail && u.Password == LoginPassword);

        if (user != null)
        {
            // ✅ Salva o ID do usuário na sessão
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserName", user.Name); // opcional

            return RedirectToPage("/Index");
        }
        else
        {
            ErrorMessage = "Email ou senha inválidos.";
            return Page();
        }
    }

}
